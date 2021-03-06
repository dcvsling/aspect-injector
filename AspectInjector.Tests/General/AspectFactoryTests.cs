﻿using AspectInjector.Broker;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspectInjector.Tests.General
{
	[TestClass]
	public class AspectFactoryTests
	{
		[TestMethod]
		public void General_AspectFactory_CreateAspect()
		{
			Checker.Passed = false;
			var test = new AspectFactoryTests_Target();
			Assert.IsTrue(Checker.Passed);
		}
	}

	[Aspect(typeof(AspectFactoryTests_Aspect))]
	public class AspectFactoryTests_Target
	{
	}

	public class AspectFactoryTests_Aspect
	{
		[Advice(InjectionPoints.After, InjectionTargets.Constructor)]
		public void TestMethod()
		{
		}

		[AspectFactory]
		public static AspectFactoryTests_Aspect Create()
		{
			Checker.Passed = true;
			return new AspectFactoryTests_Aspect();
		}
	}
}