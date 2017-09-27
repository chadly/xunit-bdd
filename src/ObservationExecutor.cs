using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Extensions
{
	public class ObservationExecutor : TestFrameworkExecutor<ObservationTestCase>
    {
        public ObservationExecutor(AssemblyName assemblyName,
                                   ISourceInformationProvider sourceInformationProvider,
                                   IMessageSink diagnosticMessageSink)
            : base(assemblyName, sourceInformationProvider, diagnosticMessageSink)
	    {
		    string config = null;
#if NET452
            config = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
#endif
		    TestAssembly = new TestAssembly(AssemblyInfo, config, assemblyName.Version);
	    }

	    /// <summary>
	    /// Gets the test assembly that contains the test.
	    /// </summary>
	    protected TestAssembly TestAssembly { get; set; }


		protected override ITestFrameworkDiscoverer CreateDiscoverer()
        {
            return new ObservationDiscoverer(AssemblyInfo, SourceInformationProvider, DiagnosticMessageSink);
        }

        protected override async void RunTestCases(IEnumerable<ObservationTestCase> testCases,
                                                   IMessageSink executionMessageSink,
                                                   ITestFrameworkExecutionOptions executionOptions)
        {
            using (var assemblyRunner = new ObservationAssemblyRunner(TestAssembly, testCases, DiagnosticMessageSink, executionMessageSink, executionOptions))
                await assemblyRunner.RunAsync();
        }
    }
}
