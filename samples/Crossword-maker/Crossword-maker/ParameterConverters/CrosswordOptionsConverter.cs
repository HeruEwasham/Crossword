using System.Collections.Generic;
using YngveHestem.Crosswords;
using YngveHestem.GenericParameterCollection;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;

namespace CrosswordMaker.ParameterConverters;

public class CrosswordOptionsConverter : ParameterCollectionParameterConverter<CrosswordOptions>
{
    protected override bool CanConvertFromParameterCollection(ParameterCollection value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
    {
        return value.HasKeyAndCanConvertTo("Board height", typeof(int), customConverters) 
        && value.HasKeyAndCanConvertTo("Board width", typeof(int), customConverters) 
        && value.HasKeyAndCanConvertTo("Words", typeof(List<WordWithInfo>), customConverters) 
        && value.HasKeyAndCanConvertTo("Glossary", typeof(List<WordWithInfo>), customConverters);
    }

    protected override bool CanConvertToParameterCollection(CrosswordOptions value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
    {
        return true;
    }

    protected override CrosswordOptions ConvertFromParameterCollection(ParameterCollection value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
    {
        return new CrosswordOptions
        {
            BoardHeight = value.GetByKey<int>("Board height", customConverters),
            BoardWidth = value.GetByKey<int>("Board width", customConverters),
            Words = value.GetByKey<List<WordWithInfo>>("Words", customConverters),
            Glossary = value.GetByKey<List<WordWithInfo>>("Glossary", customConverters)
        };
    }

    protected override ParameterCollection ConvertToParameterCollection(CrosswordOptions value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
    {
        return new ParameterCollection
        {
            { "Board height", value.BoardHeight, typeof(int), null, customConverters },
            { "Board width", value.BoardWidth, typeof(int), null, customConverters },
            { 
                "Words", value.Words, typeof(List<WordWithInfo>), new ParameterCollection
                {
                    { "SupportGetWordsFromFile", true }
                }, customConverters 
            },
            { 
                "Glossary", value.Glossary, typeof(List<WordWithInfo>), new ParameterCollection
                {
                    { "SupportGetWordsFromFile", true }
                }, customConverters 
            }
        };
    }
}
