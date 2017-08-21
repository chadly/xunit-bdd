using Xunit;
using Xunit.Extensions;

namespace Test.xUnit.BDD
{
	public class behaves_like_a_specification : Specification
	{
		protected bool observedInBase = false;

	    public override void Observe()
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

		public override void Observe()
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
}
