using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Extensions
{
	public class ObservationTestCollectionRunner : TestCollectionRunner<ObservationTestCase>
	{
		private static readonly RunSummary FailedSummary = new RunSummary { Total = 1, Failed = 1 };
		private static readonly ReaderWriterLockSlim Sync = new ReaderWriterLockSlim();
		private static readonly Dictionary<Type, bool> TypeCache = new Dictionary<Type, bool>();

		private readonly IMessageSink diagnosticMessageSink;

		public ObservationTestCollectionRunner(ITestCollection testCollection,
											   IEnumerable<ObservationTestCase> testCases,
											   IMessageSink diagnosticMessageSink,
											   IMessageBus messageBus,
											   ITestCaseOrderer testCaseOrderer,
											   ExceptionAggregator aggregator,
											   CancellationTokenSource cancellationTokenSource)
			: base(testCollection, testCases, messageBus, testCaseOrderer, aggregator, cancellationTokenSource)
		{
			this.diagnosticMessageSink = diagnosticMessageSink;
		}

		protected override async Task<RunSummary> RunTestClassAsync(ITestClass testClass,
																	IReflectionTypeInfo @class,
																	IEnumerable<ObservationTestCase> testCases)
		{
			var timer = new ExecutionTimer();
			object testClassInstance = null;

			Aggregator.Run(() => testClassInstance = Activator.CreateInstance(testClass.Class.ToRuntimeType()));

			if (Aggregator.HasExceptions)
				return FailEntireClass(testCases, timer);

			var specification = testClassInstance as ISpecification;
			if (specification == null)
			{
				Aggregator.Add(new InvalidOperationException($"Test class {testClass.Class.Name} cannot be static, and must implement ISpecification."));
				return FailEntireClass(testCases, timer);
			}

			var asynclife = specification as IAsyncLifetime;
			if (asynclife != null)
				await timer.AggregateAsync(asynclife.InitializeAsync);

			async Task ObserveAndCatchIfSpecified()
			{
				try
				{
					await specification.ObserveAsync();
				}
				catch (Exception ex)
				{
					specification.ThrownException = ex;
					if (!ShouldHandleException(specification.GetType())) throw;
				}
			}

			await Aggregator.RunAsync(ObserveAndCatchIfSpecified);
			if (Aggregator.HasExceptions)
				return FailEntireClass(testCases, timer);

			var result = await new ObservationTestClassRunner(specification, testClass, @class, testCases, diagnosticMessageSink, MessageBus, TestCaseOrderer, new ExceptionAggregator(Aggregator), CancellationTokenSource).RunAsync();

			if (asynclife != null)
				await timer.AggregateAsync(asynclife.DisposeAsync);

			if (specification is IDisposable disposable)
				timer.Aggregate(disposable.Dispose);

			return result;
		}

		private RunSummary FailEntireClass(IEnumerable<ObservationTestCase> testCases, ExecutionTimer timer)
		{
			foreach (var testCase in testCases)
			{
				MessageBus.QueueMessage(new TestFailed(new ObservationTest(testCase, testCase.DisplayName), timer.Total,
					"Exception was thrown in class constructor", Aggregator.ToException()));
			}
			int count = testCases.Count();
			return new RunSummary { Failed = count, Total = count };
		}

		private static bool ShouldHandleException(Type type)
		{
			try
			{
				Sync.EnterReadLock();

				if (TypeCache.ContainsKey(type))
					return TypeCache[type];
			}
			finally
			{
				Sync.ExitReadLock();
			}

			try
			{
				Sync.EnterWriteLock();

				if (TypeCache.ContainsKey(type))
					return TypeCache[type];

				var attrs = type.GetTypeInfo().GetCustomAttributes(typeof(HandleExceptionsAttribute), true).OfType<HandleExceptionsAttribute>();

				return TypeCache[type] = attrs.Any();
			}
			finally
			{
				Sync.ExitWriteLock();
			}
		}
	}
}