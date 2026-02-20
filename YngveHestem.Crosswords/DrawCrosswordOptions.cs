using System;
using Microsoft.Maui.Graphics;

namespace YngveHestem.Crosswords
{
	public class DrawCrosswordOptions
	{
        /// <summary>
        /// The wanted width of the crossword. Do you want the crossword on the whole canvas, this should be the same as that width.
        /// </summary>
        public float Width = 500;

        /// <summary>
        /// The wanted height of the crossword. Do you want the crossword on the whole canvas, this should be the same as that height.
        /// </summary>
        public float Height = 500;

        /// <summary>
        /// The color on the cells that should have characters in them.
        /// </summary>
        public Color CharacterCellColor = Colors.White;

        /// <summary>
        /// The color of the lines and the empty spaces.
        /// </summary>
        public Color BackgroundColor = Colors.Black;

        /// <summary>
        /// The thickness of each line.
        /// </summary>
        public float LineThickness = 1;

        /// <summary>
        /// Where on the horizontal line in the canvas should we start to draw the crossword.
        /// Do you want the crossword on the whole canvas, this should be the default 0.
        /// </summary>
        public float StartX = 0;

        /// <summary>
        /// Where on the vertical line in the canvas should we start to draw the crossword.
        /// Do you want the crossword on the whole canvas, this should be the default 0.
        /// </summary>
        public float StartY = 0;
	}
}

