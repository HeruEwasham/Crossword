using System;
using System.Numerics;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using CrosswordMaker.Dto;
using YngveHestem.Crosswords;

namespace CrosswordMaker.Views;

public class PlayCrosswordView : UserControl
{
    public bool AutoCheckChars = true;

    private Tuple<Vector2, bool>? _selectedSquare = null;
    private Square[,] _squares;

    public PlayCrosswordView(Crossword crossword, Action onNewCrossword)
    {
        var board = new UniformGrid
        {
            Rows = crossword.BoardHeight,
            Width = crossword.BoardWidth
        };
        var boardContent = crossword.GetEmptyBoard();
        _squares = new Square[crossword.BoardWidth, crossword.BoardHeight];
        for (var row = 0; row < boardContent.GetLength(0); row++)
        {
            for (var column = 0; column < boardContent.GetLength(1); column++)
            {
                var square = new Square(column, row, boardContent[column,row]);
                _squares[column, row] = square;
                board.Children.Add(square);
            }
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (_selectedSquare != null)
        {
            base.OnKeyDown(e);
            return;
        }
        if (e.Key == Key.Left)
        {
            if (_selectedSquare.Item1.X < _squares.GetLength(0)-1)
            {
                SetNewSquare(new Vector2(_selectedSquare.Item1.X+1, _selectedSquare.Item1.Y));
            }
        }
        base.OnKeyDown(e);
    }

    private void SetNewSquare(Vector2 squarePos)
    {
        DeselectSquare();
        _selectedSquare = squarePos;
        ((Border)_squares[(int)squarePos.X, (int)squarePos.Y]).BorderBrush = BRUSH_SELECTED;
    }

    private void DeselectSquare()
    {
        if (!_selectedSquare.HasValue)
        {
            return;
        }
        var square = _squares[(int)_selectedSquare.Value.X, (int)_selectedSquare.Value.Y] as Border;
        if (square.BorderBrush == BRUSH_SELECTED)
        {
            square.BorderBrush = BRUSH_BACKGROUND;
        }
        _selectedSquare = null;
    }
}
