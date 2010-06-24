using System;
using System.Xml;
using Xunit.Sdk;

namespace Xunit.Extensions
{
	public class ObservationCommand : ITestCommand
	{
		private readonly ITestCommand _innerCommand;

		public ObservationCommand(ITestCommand innerCommand)
		{
			_innerCommand = innerCommand;
		}

		public int Timeout
		{
			get { return _innerCommand.Timeout; }
		}

		public MethodResult Execute(object testClass)
		{
			// for specifications, we perform the observation before executing the test method
			var spec = testClass as ISpecification;
			if (spec != null)
				spec.Observe();

			return _innerCommand.Execute(testClass);
		}

		public bool ShouldCreateInstance
		{
			get { return _innerCommand.ShouldCreateInstance; }
		}

		public string DisplayName
		{
			get { return _innerCommand.DisplayName; }
		}

		public XmlNode ToStartXml()
		{
			return _innerCommand.ToStartXml();
		}
	}
}
