using System;

namespace Xunit.Extensions
{
	/// <summary>
	/// The base specification class
	/// </summary>
	public abstract class Specification : ISpecification
	{
		/// <summary>
		/// Performs the action to observe the outcome of to validate the specification.
		/// </summary>
		protected abstract void Observe();

		void ISpecification.Observe()
		{
			this.Observe();
		}
	}
}
