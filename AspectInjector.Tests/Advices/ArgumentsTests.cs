﻿using AspectInjector.Broker;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace AspectInjector.Tests.Advices
{
    [TestClass]
    public class ArgumentsTests
    {
        [TestMethod]
        public void AdviceArguments_Instance_InjectBeforeMethod_NotNull()
        {
            Checker.Passed = false;
            new ArgumentsTests_InstanceTarget().TestMethod();
            Assert.IsTrue(Checker.Passed);
        }

        [TestMethod]
        public void AdviceArguments_Instance_InjectBeforeStaticMethod_Null()
        {
            Checker.Passed = false;
            ArgumentsTests_StaticInstanceTarget.TestMethod();
            Assert.IsTrue(Checker.Passed);
        }

        [TestMethod]
        public void AdviceArguments_ReturnType_InjectBeforeMethod()
        {
            Checker.Passed = false;
            ArgumentsTests_ReturnTypeTarget.TestMethod();
            Assert.IsTrue(Checker.Passed);
        }

        [TestMethod]
        public void AdviceArguments_Method_InjectBeforeStaticMethod()
        {
            Checker.Passed = false;
            new ArgumentsTests_StaticMethodTarget().TestMethod();
            Assert.IsTrue(Checker.Passed);
        }

        [TestMethod]
        public void AdviceArguments_Method_InjectBeforeStaticConstructor()
        {
            Checker.Passed = false;
            var temp = new ArgumentsTests_StaticConstructorTarget();
            Assert.IsTrue(Checker.Passed);
        }

        [TestMethod]
        public void AdviceArguments_Method_InjectBeforeMethod()
        {
            Checker.Passed = false;
            new ArgumentsTests_MethodTarget().TestMethod();
            Assert.IsTrue(Checker.Passed);
        }

        [TestMethod]
        public void AdviceArguments_Method_InjectBeforeConstructor()
        {
            Checker.Passed = false;
            var temp = new ArgumentsTests_ConstructorTarget();
            Assert.IsTrue(Checker.Passed);
        }

        [TestMethod]
        public void AdviceArguments_Method_InjectBeforeConstructorChain()
        {
            Checker.Passed = false;
            new ArgumentsTests_ConstructorChainTarget().TestMethod();
            Assert.IsTrue(Checker.Passed);
        }

        [TestMethod]
        public void AdviceArguments_Method_InjectAroundMethod()
        {
            Checker.Passed = false;
            new ArgumentsTests_AroundMethodTarget().TestMethod();
            Assert.IsTrue(Checker.Passed);
        }

        [TestMethod]
        public void AdviceArguments_Arguments_InjectBeforeMethod()
        {
            var obj = new object();
            object outObj;
            var val = 1;
            int outVal;

            Checker.Passed = false;

            new ArgumentsTests_ArgumentsTarget().TestMethod(obj,
                ref obj,
                out outObj,
                val,
                ref val,
                out outVal);

            Assert.IsTrue(Checker.Passed);
        }

        [TestMethod]
        public void AdviceArguments_Arguments_InjectBeforeStaticMethod()
        {
            Checker.Passed = false;
            ArgumentsTests_StaticArgumentsTarget.TestMethod(1, "2");
            Assert.IsTrue(Checker.Passed);
        }

        internal class ArgumentsTests_InstanceTarget
        {
            [Aspect(typeof(ArgumentsTests_InstanceAspect))]
            public void TestMethod()
            {
            }
        }

        internal class ArgumentsTests_InstanceAspect
        {
            [Advice(InjectionPoints.Before, InjectionTargets.Method)]
            public void BeforeMethod([AdviceArgument(AdviceArgumentSource.Instance)] object instance)
            {
                Checker.Passed = instance != null;
            }
        }

        internal static class ArgumentsTests_StaticInstanceTarget
        {
            [Aspect(typeof(ArgumentsTests_StaticInstanceAspect))]
            public static void TestMethod()
            {
            }
        }

        internal class ArgumentsTests_StaticInstanceAspect
        {
            [Advice(InjectionPoints.Before, InjectionTargets.Method)]
            public void BeforeMethod([AdviceArgument(AdviceArgumentSource.Instance)] object instance)
            {
                Checker.Passed = instance == null;
            }
        }

        [Aspect(typeof(ArgumentsTests_ReturnTypeAspect))]
        internal static class ArgumentsTests_ReturnTypeTarget
        {
            public static void TestMethod()
            {
            }
        }

        internal class ArgumentsTests_ReturnTypeAspect
        {
            [Advice(InjectionPoints.Before, InjectionTargets.Method)]
            public void BeforeMethod([AdviceArgument(AdviceArgumentSource.Type)] System.Type type)
            {
                Checker.Passed = type == typeof(ArgumentsTests_ReturnTypeTarget);
            }
        }

        internal class ArgumentsTests_StaticMethodTarget
        {
            [Aspect(typeof(ArgumentsTests_StaticMethodAspect))]
            public void TestMethod()
            {
            }
        }

        internal class ArgumentsTests_StaticConstructorTarget
        {
            [Aspect(typeof(ArgumentsTests_StaticMethodAspect))]
            static ArgumentsTests_StaticConstructorTarget()
            {
            }
        }

        internal class ArgumentsTests_StaticMethodAspect
        {
            [Advice(InjectionPoints.Before, InjectionTargets.Method | InjectionTargets.Constructor)]
            public void BeforeMethod([AdviceArgument(AdviceArgumentSource.Method)] MethodBase method)
            {
                Checker.Passed = method != null;
            }
        }

        internal class ArgumentsTests_MethodTarget
        {
            [Aspect(typeof(ArgumentsTests_MethodAspect))]
            public void TestMethod()
            {
            }
        }

        internal class ArgumentsTests_ConstructorChainTarget
        {
            public ArgumentsTests_ConstructorChainTarget()
            {

            }

            public ArgumentsTests_ConstructorChainTarget(int a) : this()
            {

            }

            public ArgumentsTests_ConstructorChainTarget(int a, int b) : this(a)
            {

            }


            [Aspect(typeof(ArgumentsTests_MethodAspect))]
            public void TestMethod()
            {
            }
        }

        internal class ArgumentsTests_ConstructorTarget
        {
            [Aspect(typeof(ArgumentsTests_MethodAspect))]
            public ArgumentsTests_ConstructorTarget()
            {
            }
        }

        internal class ArgumentsTests_MethodAspect
        {
            [Advice(InjectionPoints.Before, InjectionTargets.Method | InjectionTargets.Constructor)]
            public void BeforeMethod([AdviceArgument(AdviceArgumentSource.Method)] MethodBase method)
            {
                Checker.Passed = method != null;
            }
        }

        internal class ArgumentsTests_ArgumentsTarget
        {
            [Aspect(typeof(ArgumentsTests_ArgumentsAspect))]
            public void TestMethod(object obj, ref object objRef, out object objOut, int value, ref int valueRef, out int valueOut)
            {
                valueOut = 1;
                objOut = new object();
            }
        }

        internal class ArgumentsTests_ArgumentsAspect
        {
            [Advice(InjectionPoints.Before, InjectionTargets.Method)]
            public void BeforeMethod([AdviceArgument(AdviceArgumentSource.Arguments)] object[] args)
            {
                Checker.Passed =
                    args[0] != null && args[1] != null && args[2] == null &&
                    (int)args[3] == 1 && (int)args[4] == 1 && (int)args[5] == 0;
            }
        }

        internal static class ArgumentsTests_StaticArgumentsTarget
        {
            [Aspect(typeof(ArgumentsTests_StaticArgumentsAspect))]
            public static void TestMethod(int a, string b)
            {
            }
        }

        internal class ArgumentsTests_StaticArgumentsAspect
        {
            [Advice(InjectionPoints.Before, InjectionTargets.Method)]
            public void BeforeMethod([AdviceArgument(AdviceArgumentSource.Arguments)] object[] args)
            {
                Checker.Passed = (int)args[0] == 1 && (string)args[1] == "2";
            }
        }

        internal class ArgumentsTests_AroundMethodTarget
        {
            [Aspect(typeof(ArgumentsTests_AroundMethodAspect))]
            public void TestMethod()
            {
            }
        }

        internal class ArgumentsTests_AroundMethodAspect
        {
            [Advice(InjectionPoints.Around, InjectionTargets.Method)]
            public object BeforeMethod([AdviceArgument(AdviceArgumentSource.Method)] MethodBase method)
            {
                Checker.Passed = method.Name == "TestMethod";
                return null;
            }
        }
    }
}