using System.Threading;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Xunit.Extensions
{
    public class ObservationTestCaseRunner : TestCaseRunner<ObservationTestCase>
    {
        readonly string displayName;
	    private readonly string skipReason;
	    readonly ISpecification specification;

        public ObservationTestCaseRunner(ISpecification specification, ObservationTestCase testCase, string displayName, string skipReason, IMessageBus messageBus, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
            : base(testCase, messageBus, aggregator, cancellationTokenSource)
        {
            this.specification = specification;
            this.displayName = displayName;
	        this.skipReason = skipReason;
        }

        protected override Task<RunSummary> RunTestAsync()
        {
            var timer = new ExecutionTimer();
            var TestClass = TestCase.TestMethod.TestClass.Class.ToRuntimeType();
            var TestMethod = TestCase.TestMethod.Method.ToRuntimeMethod();
            var test = new ObservationTest(TestCase, displayName);

            return new ObservationTestRunner(specification, test, MessageBus, timer, TestClass, TestMethod, skipReason, Aggregator, CancellationTokenSource).RunAsync();
        }
    }
}

