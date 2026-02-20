using System;
using Avalonia.Controls;
using YngveHestem.Crosswords;
using YngveHestem.GenericParameterCollection;
using YngveHestem.GenericParameterCollection.Avalonia;

namespace CrosswordMaker.Views;

public class CreateCrosswordView : UserControl
{
    public CreateCrosswordView(Action<Crossword> onCreateCrossword)
    {
        var stackPanel = new StackPanel();
        var parametersView = new ParameterCollectionView(ParameterCollection.FromObject(new CrosswordOptions(), ApplicationConstants.CustomConverters), null, ApplicationConstants.ComponentDefinitions, ApplicationConstants.CustomConverters);
        stackPanel.Children.Add(parametersView);
        var generateButton = new Button
        {
            Content = "Generate crossword",
        };
        generateButton.Click += (s,e) =>
        {
            var crossword = Crossword.Generate(parametersView.ParameterCollection.ToObject<CrosswordOptions>());
            onCreateCrossword(crossword);
        };
        stackPanel.Children.Add(generateButton);
        Content = stackPanel;
    }
}
