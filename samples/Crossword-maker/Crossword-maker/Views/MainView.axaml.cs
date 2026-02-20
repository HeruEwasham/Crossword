using Avalonia.Controls;
using YngveHestem.Crosswords;

namespace CrosswordMaker.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        CreateNewCrossword();
    }

    private void CreateNewCrossword()
    {
        Content = new CreateCrosswordView(PlayCrossword);
    }

    private void PlayCrossword(Crossword crossword)
    {
        Content = new PlayCrosswordView(crossword, () =>
        {
            CreateNewCrossword();
        });
    }
}