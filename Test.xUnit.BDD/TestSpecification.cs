using System;
using Xunit;
using Xunit.Extensions;

namespace Test.xUnit.BDD
{
	public class behaves_like_a_specification : Specification
	{
		protected bool observedInBase = false;

		protected override void Observe()
		{
			observedInBase = true;
		}

		[Observation]
		public void should_call_base_observe()
		{
			observedInBase.ShouldBeTrue("Observe should be called in the base class");
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
	public class behaves_like_a_specification_that_throws_when_observed : Specification
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

	public class behaves_like_a_specification_that_unexpectedly_throws_when_observed : Specification
	{
		protected override void Observe()
		{
			throw new TestException();
		}

		[Observation]
		public void should_fail()
		{
			// This test will fail because of the exception thrown in Observe().
		}
	}
}
