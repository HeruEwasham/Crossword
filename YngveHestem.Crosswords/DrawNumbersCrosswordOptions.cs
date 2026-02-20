using Microsoft.Maui.Graphics;

namespace YngveHestem.Crosswords
{
    public class DrawNumbersCrosswordOptions : DrawCrosswordOptions
    {
        /// <summary>
        /// The font to use to draw the numbers on the board.
        /// </summary>
        public Font NumberFont = Font.Default;

        /// <summary>
        /// The color to use when the number is drawn.
        /// </summary>
        public Color NumberFontColor = Colors.Black;

        /// <summary>
        /// The font size to use when the number is drawn.
        /// </summary>
        public float NumberFontSize = 10;

        /// <summary>
        /// How long horizontally from the upper left of the cell should the number be placed.
        /// </summary>
        public float NumberPlacementX = 0;

        /// <summary>
        /// How long vertically from the upper left of the cell should the number be placed.
        /// </summary>
        public float NumberPlacementY = 0;
    }
}