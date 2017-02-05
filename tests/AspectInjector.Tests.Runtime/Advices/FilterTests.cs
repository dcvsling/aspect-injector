﻿using AspectInjector.Broker;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspectInjector.Tests.Advices
{
    [TestClass]
    public class FilterTests
    {
        [TestMethod]
        public void Advices_InjectAfterMethod_NameFilter()
        {
            Checker.Passed = false;

            var a = new FilterTests_Target();
            a.Do123();

            Assert.IsTrue(Checker.Passed);
        }

        [Inject(typeof(FilterTests_Aspect))]
        public class FilterTests_Target
        {
            [Inject(typeof(FilterTests_Aspect)/*, NameFilter = "Do"*/)]
            public void Do123()
            {
            }
        }

        [Aspect(Aspect.Scope.Global)]
        public class FilterTests_Aspect
        {
            public int Counter = 0;

            [Advice(Advice.Type.After, Advice.Target.Method)]
            public void AfterMethod()
            {
                Counter++;
                Checker.Passed = Counter == 1;
            }
        }
    }
}