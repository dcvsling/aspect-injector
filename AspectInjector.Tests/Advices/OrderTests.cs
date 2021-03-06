﻿using AspectInjector.Broker;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspectInjector.Tests.Advices
{
    [TestClass]
    public class OrderTests
    {
        private OrderTests_Target _beforeTestClass;

        [TestInitialize]
        public void SetUp()
        {
            _beforeTestClass = new OrderTests_Target();
        }

        [TestMethod, Ignore]//in release the order may be changed by compiller :(
        public void Advices_InjectBeforeMethod_Ordered()
        {
            Checker.Passed = false;
            _beforeTestClass.TestMethod();
            Assert.IsTrue(Checker.Passed);
        }
    }

    [Aspect(typeof(OrderTests_Aspect1))]
    [Aspect(typeof(OrderTests_Aspect2))]
    [Aspect(typeof(OrderTests_Aspect3))]
    internal class OrderTests_Target
    {
        public void TestMethod()
        {
        }
    }

    internal class OrderTests_Aspect1
    {
        [Advice(InjectionPoints.Before, InjectionTargets.Method)]
        public void BeforeMethod()
        {
        }
    }

    internal class OrderTests_Aspect2
    {
        [Advice(InjectionPoints.Before, InjectionTargets.Method)]
        public void BeforeMethod()
        {
            Checker.Passed = false;
        }
    }

    internal class OrderTests_Aspect3
    {
        [Advice(InjectionPoints.Before, InjectionTargets.Method)]
        public void BeforeMethod()
        {
            Checker.Passed = true;
        }
    }
}