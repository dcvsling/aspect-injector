﻿using AspectInjector.Core.Configuration;

namespace AspectInjector.Core.Mixin
{
    public static class InterfaceProxyConfigurationExtensions
    {
        public static ProcessingConfiguration UseMixinInjections(this ProcessingConfiguration config)
        {
            return config
                .RegisterInjectionReader<MixinReader>()
                .RegisterInjector<MixinInjector>();
        }
    }
}