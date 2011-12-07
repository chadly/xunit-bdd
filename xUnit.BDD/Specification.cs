using System;

namespace Xunit.Extensions
{
	/// <summary>
	/// The base specification class
	/// </summary>
	public abstract class Specification : ISpecification
	{
		readonly ExceptionHandlingMode exceptionMode;
		Exception exception;

		public ExceptionHandlingMode ExceptionMode
		{
			get { return exceptionMode; }
		}

		/// <summary>
		/// The exception that was thrown when Observe was run; null if no exception was thrown.
		/// </summary>
		protected Exception ThrownException
		{
			get { return exception; }
		}

		public Specification()
			: this(ExceptionHandlingMode.Throw) { }

		public Specification(ExceptionHandlingMode exceptionMode)
		{
			this.exceptionMode = exceptionMode;
		}

		/// <summary>
		/// Performs the action to observe the outcome of to validate the specification.
		/// </summary>
		public abstract void Observe();

		void ISpecification.SetException(Exception ex)
		{
			exception = ex;
		}
	}
}