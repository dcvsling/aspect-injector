﻿using AspectInjector.Broker;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace AspectInjector.Tests.Advices
{
    [TestClass]
    public class AroundTests
    {
        [TestMethod]
        public void Advices_InjectAroundMethod_StructResult()
        {
            Checker.Passed = false;

            var a = new AroundTests_Target();

            object ref1 = new object();
            object out1;
            int ref2 = 2;
            int out2;

            a.Do1(new object(), 1, ref ref1, out out1, ref ref2, out out2, false, false);

            Assert.IsTrue(Checker.Passed);
        }

        [TestMethod]
        public void Advices_InjectAroundMethod()
        {
            Checker.Passed = false;

            var a = new AroundTests_Target();

            object ref1 = new object();
            object out1;
            int ref2 = 2;
            int out2;

            a.Do2(new object(), 1, ref ref1, out out1, ref ref2, out out2, false, false);

            Assert.IsTrue(Checker.Passed);
        }

        [TestMethod]
        public void Advices_InjectAroundMethod_ModifyArguments()
        {
            var i = 1;

            Checker.Passed = true;
            new AroundTests_ArgumentsModificationTarget().TestMethod(ref i);
            Assert.IsTrue(Checker.Passed);
        } 

        [TestMethod, Ignore]
        public void Advices_InjectAroundMethod_NoWrapperInStackTrace()
        {
            var passed = false;

            var a = new AroundTests_StackTraceTarget();

            try
            {
                a.Do();
            }
            catch (Exception e)
            {
                passed = !e.StackTrace.Contains("__a$");
            }

            Assert.IsTrue(passed);
        }

        internal class AroundTests_Target
        {
            [Aspect(typeof(AroundTests_Aspect1))]
            [Aspect(typeof(AroundTests_Aspect2))] //fire first
            public object Do2(object data, int value, ref object testRef, out object testOut, ref int testRefValue, out int testOutValue, bool passed, bool passed2)
            {
                Checker.Passed = passed && passed2;

                testOut = new object();
                testOutValue = 1;

                return new object();
            }

            [Aspect(typeof(AroundTests_Aspect1))]
            [Aspect(typeof(AroundTests_Aspect2))] //fire first
            public int Do1(object data, int value, ref object testRef, out object testOut, ref int testRefValue, out int testOutValue, bool passed, bool passed2)
            {
                Checker.Passed = passed && passed2;

                testOut = new object();
                testOutValue = 1;

                return 1;
            }
        }

        internal class AroundTests_Aspect1
        {
            [Advice(InjectionPoints.Around, InjectionTargets.Method)]
            public object AroundMethod([AdviceArgument(AdviceArgumentSource.Target)] Func<object[], object> target,
                [AdviceArgument(AdviceArgumentSource.Arguments)] object[] arguments)
            {
                return target(new object[] { arguments[0], arguments[1], arguments[2], arguments[3], arguments[4], arguments[5], true, arguments[7] });
            }
        }

        internal class AroundTests_Aspect2
        {
            [Advice(InjectionPoints.Around, InjectionTargets.Method)]
            public object AroundMethod([AdviceArgument(AdviceArgumentSource.Target)] Func<object[], object> target,
                [AdviceArgument(AdviceArgumentSource.Arguments)] object[] arguments)
            {
                return target(new object[] { arguments[0], arguments[1], arguments[2], arguments[3], arguments[4], arguments[5], arguments[6], true });
            }
        }

        internal class AroundTests_ArgumentsModificationTarget
        {
            [Aspect(typeof(AroundTests_ArgumentsModificationAspect))]
            public void TestMethod(ref int i)
            {
                if (i == 2)
                    i = 3;
            }
        }

        internal class AroundTests_ArgumentsModificationAspect
        {
            [Advice(InjectionPoints.Around, InjectionTargets.Method)]
            public object AroundMethod(
                [AdviceArgument(AdviceArgumentSource.Arguments)] object[] args,
                [AdviceArgument(AdviceArgumentSource.Target)] Func<object[], object> target
                )
            {
                args[0] = 2;

                var result = target(args);

                Checker.Passed = (int)args[0] == 3;

                return result;
            }
        }

        internal class AroundTests_StackTraceTarget
        {
            [Aspect(typeof(AroundTests_StackTraceAspect))]
            public void Do()
            {
                throw new Exception("Test Exception");
            }
        }

        public class AroundTests_StackTraceAspect
        {
            [Advice(InjectionPoints.Around, InjectionTargets.Method)]
            public object AroundMethod([AdviceArgument(AdviceArgumentSource.Target)] Func<object[], object> target,
                [AdviceArgument(AdviceArgumentSource.Arguments)] object[] arguments)
            {
                return target(arguments);
            }
        }
    }
}