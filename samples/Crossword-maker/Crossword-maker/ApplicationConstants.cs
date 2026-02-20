using System;
using CrosswordMaker.ParameterConverters;
using YngveHestem.GenericParameterCollection.Avalonia.ParameterComponents;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;

namespace CrosswordMaker;

public static class ApplicationConstants
{
    public static IParameterValueConverter[] CustomConverters = new IParameterValueConverter[]
    {
        new CrosswordOptionsConverter(),
        new WordWithInfoConverter()
    };

    public static IParameterComponentDefinition[] ComponentDefinitions = new IParameterComponentDefinition[]
    {
        
    };
}
