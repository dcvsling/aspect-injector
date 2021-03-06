﻿using AspectInjector.Broker;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace AspectInjector.Tests.Advices
{
    [TestClass]
    public class AsyncTests
    {
        public static bool Data = false;

        [TestMethod]
        public void Advices_InjectAfterAsyncMethod()
        {
            AsyncTests.Data = false;
            Checker.Passed = false;

            var a = new AsyncTests_Target();
            a.Do().Wait();

            Assert.IsTrue(Checker.Passed);
        }

        [TestMethod]
        public void Advices_InjectAfterAsyncMethod_WithResult()
        {
            AsyncTests.Data = false;
            Checker.Passed = false;

            var a = new AsyncTests_Target();
            var result = a.Do2().Result;

            Assert.AreEqual(result, "test");
        }

        [TestMethod]
        public void Advices_InjectAfterAsyncMethod_Void()
        {
            AsyncTests.Data = false;
            Checker.Passed = false;

            var a = new AsyncTests_Target();
            a.Do3();
            Task.Delay(200).Wait();

            Assert.IsTrue(Checker.Passed);
        }

        [TestMethod]
        public void Advices_InjectAfterAsyncMethod_WithArguments()
        {
            AsyncTests.Data = false;
            Checker.Passed = false;

            var a = new AsyncTests_Target();
            a.Do4("args_test").Wait();

            Assert.IsTrue(Checker.Passed);
        }
    }

    public class AsyncTests_Target
    {
        [Aspect(typeof(AsyncTests_SimpleAspect))]
        public async Task Do()
        {
            await Task.Delay(1);

            AsyncTests.Data = true;
        }

        [Aspect(typeof(AsyncTests_SimpleAspect))]
        public async Task<string> Do2()
        {
            await Task.Delay(1);

            AsyncTests.Data = true;

            return "test";
        }

        [Aspect(typeof(AsyncTests_SimpleAspect))]
        public async void Do3()
        {
            await Task.Delay(1);

            AsyncTests.Data = true;
        }

        [Aspect(typeof(AsyncTests_ArgumentsAspect))]
        public async Task<string> Do4(string testData)
        {
            await Task.Delay(1);

            AsyncTests.Data = true;

            return testData;
        }

        public class AsyncTests_ArgumentsAspect
        {
            [Advice(InjectionPoints.After, InjectionTargets.Method)]
            public void AfterMethod([AdviceArgument(AdviceArgumentSource.ReturnValue)] object value,
                [AdviceArgument(AdviceArgumentSource.Arguments)] object[] args
                )
            {
                Checker.Passed = args[0].ToString() == "args_test";
            }
        }

        public class AsyncTests_SimpleAspect
        {
            [Advice(InjectionPoints.After, InjectionTargets.Method)]
            public void AfterMethod([AdviceArgument(AdviceArgumentSource.ReturnValue)] object value,
                [AdviceArgument(AdviceArgumentSource.Arguments)] object[] args
                )
            {
                Checker.Passed = AsyncTests.Data;
            }
        }
    }
}