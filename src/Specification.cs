using System.Threading.Tasks;

namespace Xunit.Extensions
{
	/// <summary>
	/// The base specification class for non-async scenarios
	/// </summary>
	public abstract class Specification : AsyncSpecification
	{
		protected sealed override Task InitializeAsync() => base.InitializeAsync();
		protected sealed override Task DisposeAsync() => base.DisposeAsync();

		protected sealed override Task ObserveAsync()
		{
			Observe();
			return CommonTasks.Completed;
		}

		/// <summary>
		/// Performs the action to observe the outcome of to validate the specification.
		/// </summary>
		protected abstract void Observe();
	}
}