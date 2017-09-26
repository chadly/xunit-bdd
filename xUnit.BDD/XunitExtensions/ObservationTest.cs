using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Extensions
{
    public class ObservationTest : LongLivedMarshalByRefObject, ITest
    {
        public ObservationTest(ObservationTestCase testCase, string displayName)
        {
            TestCase = testCase;
            DisplayName = displayName;
        }

        public string DisplayName { get; private set; }

        public ObservationTestCase TestCase { get; private set; }

        ITestCase ITest.TestCase { get { return TestCase; } }
    }
}
