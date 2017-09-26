using System;

namespace Xunit.Extensions
{
	/// <summary>
	/// Identifies a method as an observation which asserts the specification
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
    public class ObservationAttribute : Attribute
	{
		/// <summary>
		/// Gets the name of the test to be used when the test is skipped. Defaults to
		/// null, which will cause the fully qualified test name to be used.
		/// </summary>
		public virtual string DisplayName { get; set; }

		/// <summary>
		/// Marks the test so that it will not be run, and gets or sets the skip reason
		/// </summary>
		public virtual string Skip { get; set; }
	}
}
