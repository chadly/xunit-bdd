using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;
using System.Linq;

namespace Xunit.Extensions
{
    public class ObservationTestCase : TestMethodTestCase
    {
        [Obsolete("For de-serialization purposes only", error: true)]
        public ObservationTestCase() { }

        public ObservationTestCase(TestMethodDisplay defaultMethodDisplay, ITestMethod testMethod)
            : base(defaultMethodDisplay, testMethod) { }

        protected override void Initialize()
        {
            base.Initialize();

	        IAttributeInfo observationAttribute = TestMethod.Method.GetCustomAttributes(typeof(ObservationAttribute)).First();
			
			DisplayName = $"{TestMethod.TestClass.Class.Name}.{TestMethod.Method.Name}";
	        SkipReason = GetSkipReason(observationAttribute);
		}

		/// <summary>
		/// Gets the skip reason for the test case. By default, pulls the skip reason from the
		/// <see cref="ObservationAttribute.Skip"/> property.
		/// </summary>
		/// <param name="observationAttribute">The observation attribute that decorated the test case.</param>
		/// <returns>The skip reason, if skipped; <c>null</c>, otherwise.</returns>
		protected virtual string GetSkipReason(IAttributeInfo observationAttribute)
		    => observationAttribute.GetNamedArgument<string>("Skip");

		public Task<RunSummary> RunAsync(Specification specification,
                                         IMessageBus messageBus,
                                         ExceptionAggregator aggregator,
                                         CancellationTokenSource cancellationTokenSource)
        {
            return new ObservationTestCaseRunner(specification, this, DisplayName, SkipReason, messageBus, aggregator, cancellationTokenSource).RunAsync();
        }
    }
}