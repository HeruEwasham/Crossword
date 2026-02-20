using System.Collections.Generic;
using YngveHestem.Crosswords;
using YngveHestem.GenericParameterCollection;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;

namespace CrosswordMaker.ParameterConverters;

public class WordWithInfoConverter : ParameterCollectionParameterConverter<WordWithInfo>
{
    protected override bool CanConvertFromParameterCollection(ParameterCollection value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
    {
        return value.HasKeyAndCanConvertTo("Word", typeof(string), customConverters) && value.HasKeyAndCanConvertTo("Hint", typeof(string), customConverters);
    }

    protected override bool CanConvertToParameterCollection(WordWithInfo value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
    {
        return true;
    }

    protected override WordWithInfo ConvertFromParameterCollection(ParameterCollection value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
    {
        return new WordWithInfo(value.GetByKey<string>("Word", customConverters) ?? string.Empty, value.GetByKey<string>("Hint", customConverters));
    }

    protected override ParameterCollection ConvertToParameterCollection(WordWithInfo value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
    {
        return new ParameterCollection
        {
            { "Word", value.Word, typeof(string) },
            { "Hint", value.Hint, typeof(string) }
        };
    }

    protected override WordWithInfo GetDefaultValue(IEnumerable<WordWithInfo> value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
    {
        return new WordWithInfo(string.Empty, null);
    }
}
