using System;
using Xunit;

[assembly: TestFramework("Xunit.Extensions.ObservationTestFramework", "Xunit.Bdd")]

namespace Xunit.Extensions
{
	/// <summary>
	/// Identifies a method as an observation which asserts the specification
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
    public class ObservationAttribute : Attribute
	{
		/// <summary>
		/// Marks the test so that it will not be run, and gets or sets the skip reason
		/// </summary>
		public string Skip { get; set; }
	}
}
