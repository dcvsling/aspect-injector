﻿using AspectInjector.Core.Contracts;
using AspectInjector.Core.Extensions;
using Mono.Cecil;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Linq;
using static AspectInjector.Broker.Aspect;

namespace AspectInjector.Core.Models
{
    public class AspectDefinition
    {
        private MethodDefinition _factoryMethod;

        public TypeDefinition Host { get; set; }

        public List<Effect> Effects { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Scope Scope { get; set; }

        public TypeReference Factory { get; set; }

        public MethodReference GetFactory()
        {
            if (_factoryMethod == null)
            {
                if (Factory != null)
                {
                    _factoryMethod = Factory.Resolve().Methods.FirstOrDefault(m =>
                    m.IsStatic && m.IsPublic
                    && m.Name == Constants.AspectFactoryMethodName
                    && m.ReturnType.GetFQN() == FQN.Object
                    && m.Parameters.Count == 1 && m.Parameters[0].ParameterType.GetFQN() == FQN.Type
                    );
                }
                else
                    _factoryMethod = Host.Methods.FirstOrDefault(m => m.IsConstructor && !m.IsStatic && m.IsPublic && !m.HasParameters);
            }

            return _factoryMethod;
        }

        public bool Validate(ILogger log)
        {
            //if (!Host.Is)
            //    log.LogWarning(CompilationMessage.From($"Aspect {Host.FullName} is not public.", Host));

            if (!Effects.Any())
                log.LogWarning(CompilationMessage.From($"Type {Host.FullName} has defined as an aspect, but lacks any effect.", Host));

            if (Host.HasGenericParameters)
            {
                log.LogError(CompilationMessage.From($"Aspect {Host.FullName} should not have generic parameters.", Host));
                return false;
            }

            if (GetFactory() == null)
            {
                if (Factory != null)
                    log.LogError(CompilationMessage.From($"Type {Factory.FullName} should have 'public static object GetInstance(Type)' method in order to be aspect factory.", Host));
                else
                    log.LogError(CompilationMessage.From($"Aspect {Host.FullName} has no parameterless public constructor nor valid factory.", Host.Methods.First(m => m.IsConstructor && !m.IsStatic)));
                return false;
            }

            return Effects.All(e => e.Validate(this, log));
        }
    }
}