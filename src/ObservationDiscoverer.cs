using System;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Extensions
{
	public class ObservationDiscoverer : TestFrameworkDiscoverer
    {
        readonly CollectionPerClassTestCollectionFactory testCollectionFactory;

        public ObservationDiscoverer(IAssemblyInfo assemblyInfo,
                                     ISourceInformationProvider sourceProvider,
                                     IMessageSink diagnosticMessageSink)
            : base(assemblyInfo, sourceProvider, diagnosticMessageSink)
		{
			string config = null;
#if NET452
            config = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
#endif

			var testAssembly = new TestAssembly(assemblyInfo, config);
            testCollectionFactory = new CollectionPerClassTestCollectionFactory(testAssembly, diagnosticMessageSink);
        }

        protected override ITestClass CreateTestClass(ITypeInfo @class)
        {
            return new TestClass(testCollectionFactory.Get(@class), @class);
        }

        bool FindTestsForMethod(ITestMethod testMethod,
                                TestMethodDisplay defaultMethodDisplay,
                                bool includeSourceInformation,
                                IMessageBus messageBus)
        {
            var observationAttribute = testMethod.Method.GetCustomAttributes(typeof(ObservationAttribute)).FirstOrDefault();
            if (observationAttribute == null)
                return true;

            var testCase = new ObservationTestCase(defaultMethodDisplay, testMethod);
            if (!ReportDiscoveredTestCase(testCase, includeSourceInformation, messageBus))
                return false;

            return true;
        }

        protected override bool FindTestsForType(ITestClass testClass,
                                                 bool includeSourceInformation,
                                                 IMessageBus messageBus,
                                                 ITestFrameworkDiscoveryOptions discoveryOptions)
        {
            var methodDisplay = discoveryOptions.MethodDisplayOrDefault();

            foreach (var method in testClass.Class.GetMethods(includePrivateMethods: true))
                if (!FindTestsForMethod(new TestMethod(testClass, method), methodDisplay, includeSourceInformation, messageBus))
                    return false;

            return true;
        }
    }
}
