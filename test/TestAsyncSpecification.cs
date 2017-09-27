using System.Threading.Tasks;
using Xunit.Extensions;

namespace Xunit.Bdd.Test
{
	public class behaves_like_an_async_specification : AsyncSpecification, IAsyncLifetime
	{
		protected static int constructionCount = 0;
		protected static int initCount = 0;
		protected static int disposeCount = 0;

		int observedCount = 0;

		public behaves_like_an_async_specification()
		{
			constructionCount++;
		}

		public Task InitializeAsync()
		{
			initCount++;
			return Task.CompletedTask;
		}

		public Task DisposeAsync()
		{
			disposeCount++;
			return Task.CompletedTask;
		}

		public override Task ObserveAsync()
		{
			observedCount++;
			return Task.CompletedTask;
		}

		[Observation]
		public void should_call_observe_once()
		{
			observedCount.ShouldEqual(1);
		}

		[Observation]
		public void should_call_init_only_once()
		{
			initCount.ShouldEqual(1);
		}

		[Observation]
		public void should_construct_only_once()
		{
			constructionCount.ShouldEqual(1);
		}
	}
}
