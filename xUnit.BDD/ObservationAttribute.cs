using System;
using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Extensions
{
	/// <summary>
	/// Identifies a method as an observation which asserts the specification
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class ObservationAttribute : FactAttribute
	{
		protected override IEnumerable<ITestCommand> EnumerateTestCommands(IMethodInfo method)
		{
			foreach (ITestCommand command in EnumerateTestCommandsInternal(method))
			{
				yield return new ObservationCommand(command);
			}
		}

		private IEnumerable<ITestCommand> EnumerateTestCommandsInternal(IMethodInfo method)
		{
			//this extra method is to avoid compiler warning
			return base.EnumerateTestCommands(method);
		}
	}
}