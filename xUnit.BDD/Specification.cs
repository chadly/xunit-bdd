using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Xunit.Extensions
{
	/// <summary>
	/// The base specification class
	/// </summary>
	public abstract class Specification : ISpecification
	{
		private Exception exception;

		/// <summary>
		/// The exception that was thrown when Observe was run; null if no exception was thrown.
		/// </summary>
		protected Exception ThrownException => exception;

		/// <summary>
		/// Performs the action to observe the outcome of to validate the specification.
		/// </summary>
		public abstract void Observe();

		bool ISpecification.HandleException(Exception ex)
		{
			exception = ex;
			return ShouldHandleException();
		}

		private static readonly ReaderWriterLockSlim sync = new ReaderWriterLockSlim();
		private static readonly Dictionary<Type, bool> typeCache = new Dictionary<Type, bool>();

		bool ShouldHandleException()
		{
			Type type = GetType();

			try
			{
				sync.EnterReadLock();

				if (typeCache.ContainsKey(type))
					return typeCache[type];
			}
			finally
			{
				sync.ExitReadLock();
			}

			try
			{
				sync.EnterWriteLock();

				if (typeCache.ContainsKey(type))
					return typeCache[type];

				var attrs = type.GetTypeInfo().GetCustomAttributes(typeof(HandleExceptionsAttribute), true).OfType<HandleExceptionsAttribute>();

				return typeCache[type] = attrs.Any();
			}
			finally
			{
				sync.ExitWriteLock();
			}
		}

		public Task InitializeAsync()
		{
			Observe();
			return CommonTasks.Completed;
		}

		public Task DisposeAsync()
		{
			return CommonTasks.Completed;
		}
	}
}