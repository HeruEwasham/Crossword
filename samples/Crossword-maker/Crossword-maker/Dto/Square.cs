using System;
using System.Net;
using System.Numerics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using YngveHestem.Crosswords;

namespace CrosswordMaker.Dto;

public class Square : UserControl
{
    public const int RECTANGLE_SIZE = 50;
    public const int BORDER_THICKNESS = 1;
    
    public static IBrush BRUSH_SELECTED = Brushes.Blue;
    public static IBrush BRUSH_HOVER = Brushes.LightBlue;
    public static IBrush BRUSH_ERROR = Brushes.Red;
    public static IBrush BRUSH_BACKGROUND = Brushes.Black;

    public static Rectangle BLACK_RECTANGLE = new Rectangle
    {
        Width = RECTANGLE_SIZE,
        Height = RECTANGLE_SIZE,
        Fill = BRUSH_BACKGROUND,
        Stroke = BRUSH_BACKGROUND,
        StrokeThickness = BORDER_THICKNESS
    };
    
    public char? Character { get; }

    public SquareStatus Status { get; }

    public Vector2 Placement { get; }

    public Square(int column, int row, char desc)
    {
        Placement = new Vector2(column, row);
        if (desc == Crossword.BLACK_BOX_CHARACTER)
        {
            Content = BLACK_RECTANGLE;
            Status = SquareStatus.Black;
        }
        else if (desc == Crossword.WHITE_BOX_CHARACTER)
        {
            Content = GetWhiteRectangle(column, row);
            Status = SquareStatus.WhiteWithoutCharacter;
        }
        else
        {
            Content = GetWhiteRectangle(column, row, desc);
        }
    }

    private Border GetWhiteRectangle(int column, int row, char? character = null)
    {
        var rectangle = new Border
        {
            Width = RECTANGLE_SIZE,
            Height = RECTANGLE_SIZE,
            BorderBrush = BRUSH_BACKGROUND,
            BorderThickness = new Thickness(BORDER_THICKNESS),
            Child = new TextBlock
            {
                Text = character == null ? string.Empty : character.ToString(),
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
            }
        };
        rectangle.PointerPressed += (s,e) =>
        {
            SetNewSquare(new Vector2(column, row));
        };
        rectangle.PointerEntered += (s,e) =>
        {
            rectangle.BorderBrush = BRUSH_HOVER;
        };
        rectangle.PointerExited += (s,e) =>
        {
            rectangle.BorderBrush = BRUSH_BACKGROUND;
        };
        return rectangle;
    }
}
