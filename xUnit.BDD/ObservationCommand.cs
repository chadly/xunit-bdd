using System;
using System.Xml;
using Xunit.Sdk;

namespace Xunit.Extensions
{
	public class ObservationCommand : ITestCommand
	{
		private readonly ITestCommand _innerCommand;

		public static event EventHandler Observing;
		public static event EventHandler Observed;

		public static event EventHandler Executing;
		public static event EventHandler Executed;

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
			{
				OnObserving();
				try
				{
					spec.Observe();
				}
				catch (Exception ex)
				{
					if (!spec.HandleException(ex))
						throw;
				}
				OnObserved();
			}

			OnExecuting();
			MethodResult result = _innerCommand.Execute(testClass);
			OnExecuted();

			return result;
		}

		private void OnObserving()
		{
			if (Observing != null)
				Observing(this, EventArgs.Empty);
		}

		private void OnObserved()
		{
			if (Observed != null)
				Observed(this, EventArgs.Empty);
		}

		private void OnExecuting()
		{
			if (Executing != null)
				Executing(this, EventArgs.Empty);
		}

		private void OnExecuted()
		{
			if (Executed != null)
				Executed(this, EventArgs.Empty);
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