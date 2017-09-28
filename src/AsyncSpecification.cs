using System;
using System.Threading.Tasks;

namespace Xunit.Extensions
{
	/// <summary>
	/// The base async specification class
	/// </summary>
	public abstract class AsyncSpecification : ISpecification, IAsyncLifetime
	{
		/// <summary>
		/// The exception that was thrown when Observe was run; null if no exception was thrown.
		/// </summary>
		public Exception ThrownException { get; set; }

		public virtual Task InitializeAsync()
		{
			return Task.FromResult(0);
		}

		/// <summary>
		/// Performs an action, the outcome of which will be observed to validate the specification.
		/// </summary>
		public abstract Task ObserveAsync();

		public virtual Task DisposeAsync()
		{
			return Task.FromResult(0);
		}
	}
}