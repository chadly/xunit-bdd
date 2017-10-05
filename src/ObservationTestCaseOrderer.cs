using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Extensions
{
	public class ObservationTestCaseOrderer : ITestCaseOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
			return testCases
				.OrderBy(c => c.TestMethod.TestClass.Class.Name)
				.ThenBy(c => c.TestMethod.Method.Name)
				.ToList();
        }
    }
}
