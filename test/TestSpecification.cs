using System;
using Xunit;

[assembly: TestFramework("Xunit.ObservationTestFramework", "Xunit.Bdd")]

namespace Xunit.Bdd.Test
{
	public class behaves_like_a_specification : Specification
	{
		private bool beforeObserveCalled = false;
		private bool wasObservedAtBeforeObserve = true;

		private bool observedInBase = false;

		private bool afterObserveCalled = false;
		private bool wasObservedAtAfterObserve = false;

		protected override void BeforeObserve()
		{
			beforeObserveCalled = true;
			wasObservedAtBeforeObserve = observedInBase;
		}

		protected override void Observe()
		{
			observedInBase = true;
		}

		protected override void AfterObserve()
		{
			afterObserveCalled = true;
			wasObservedAtAfterObserve = observedInBase;
		}

		[Observation]
		public void should_call_base_observe()
		{
			observedInBase.ShouldBeTrue("Observe should be called in the base class");
		}

		[Observation]
		public void should_call_beforeobserve_before_observing()
		{
			beforeObserveCalled.ShouldBeTrue("BeforeObserve should have been called");
			wasObservedAtBeforeObserve.ShouldBeFalse("BeforeObserve should have been called before Observe");
		}

		[Observation]
		public void should_call_afterobserve_after_observing()
		{
			afterObserveCalled.ShouldBeTrue("AfterObserve should have been called");
			wasObservedAtAfterObserve.ShouldBeTrue("AfterObserve should have been called after Observe");
		}

		[Observation(Skip="Skipped this observation")]
		public void should_skip_this_observation()
		{
			Assert.True(false);
		}
	}

	public class behaves_like_a_polymorphic_specification : behaves_like_a_specification
	{
		protected bool observedInDerived = false;

		protected override void Observe()
		{
			base.Observe();
			observedInDerived = true;
		}

		[Observation]
		public void should_call_derived_observe()
		{
			observedInDerived.ShouldBeTrue("Observe should be called in the derived class");
		}
	}

	public class TestException : Exception { }

	[HandleExceptions]
	public class behaves_like_a_specification_that_throws_during_setup : Specification
	{
		protected override void Observe()
		{
			throw new TestException();
		}

		[Observation]
		public void should_handle_exception()
		{
			ThrownException.ShouldNotBeNull();
			ThrownException.ShouldBeType<TestException>();
		}
	}

	public class behaves_like_a_specification_that_unexpectedly_throws_during_setup : Specification
	{
		protected override void Observe()
		{
			throw new TestException();
		}

		[Observation]
		public void should_be_inconclusive()
		{
			// This test will have an inconclusive result because of the exception thrown in Observe()
		}

		[Observation(Skip = "YOU SHOULD NEVER SEE THIS AS A TEST RESULT")] // The runner can't reach the point where it skips a test if its setup can't be run first.
		public void should_still_be_inconclusive_even_if_skipped()
		{
			// This test will have an inconclusive result because of the exception thrown in Observe()
		}
	}
}
