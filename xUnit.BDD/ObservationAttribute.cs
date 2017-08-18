using System;
using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Extensions
{
	/// <summary>
	/// Identifies a method as an observation which asserts the specification
	/// </summary>
	public class ObservationAttribute : FactAttribute
	{
	}
}