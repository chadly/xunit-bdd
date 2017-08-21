using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit.Extensions;

namespace Test.xUnit.BDD
{
	// What a mess
    class TestISpecification : ISpecification
    {
		// xUnit calls this method before the test is run
	    public Task InitializeAsync()
		{
			Observe(); // Must call Observe here otherwise test won't behave as expected.
			return Task.CompletedTask;
	    }

	    public Task DisposeAsync()
		{
			return Task.CompletedTask;
		}

	    private bool wasObserved = false;

		// This method is ignored by xUnit
	    public void Observe()
	    {
		    wasObserved = true;
	    }

	    public bool HandleException(Exception ex)
	    {
		    return false;
	    }

	    [Observation]
	    public void should_call_observe()
	    {
		    wasObserved.ShouldBeTrue("Observe() should be called");
	    }
    }
}
