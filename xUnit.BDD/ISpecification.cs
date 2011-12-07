using System;

namespace Xunit.Extensions
{
	/// <summary>
	/// The specification interface
	/// </summary>
	public interface ISpecification
	{
		/// <summary>
		/// Performs the action to observe the outcome of to validate the specification.
		/// </summary>
		void Observe();

		/// <summary>
		/// Sets any resulting exceptions that are a result of the observe method.
		/// </summary>
		void SetException(Exception ex);
	}
}