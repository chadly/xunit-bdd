using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace Xunit.Extensions
{
	/// <summary>
	/// Identifies a method as an observation which asserts the specification
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class ObservationAttribute : FactAttribute
	{
		protected override IEnumerable<ITestCommand> EnumerateTestCommands(MethodInfo method)
		{
			foreach (ITestCommand command in base.EnumerateTestCommands(method))
			{
				yield return new ObservationCommand(command);
			}
		}
	}
}
