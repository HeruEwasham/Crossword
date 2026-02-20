using Microsoft.Maui.Graphics;

namespace YngveHestem.Crosswords
{
    public class DrawCharactersOptions
	{
        /// <summary>
        /// The font to use to draw the characters with.
        /// </summary>
        public Font CharacterFont = Font.Default;

        /// <summary>
        /// The color to use when the character is drawn.
        /// </summary>
		public Color CharacterColor = Colors.Black;

        /// <summary>
        /// How many pixels from the left of the cell should the character be drawn.
        /// If this is set to null, the horizontal placement will be placed in the middle of the cell (as long as CharacterHorizontalAlignment is set to Center).
        /// </summary>
        public float? CharacterPlacementX = null;

        /// <summary>
        /// How many pixels above the bottom of the cell should the bottom of the character be.
        /// </summary>
        public float CharacterPlacementY = 1;

        /// <summary>
        /// How big should the font size be? If 0 or less than 0, default size will be used.
        /// </summary>
        public float CharacterFontSize = 0;

        /// <summary>
        /// Which horizontal alignment should be used to draw the character. 
        /// </summary>
        public HorizontalAlignment CharacterHorizontalAlignment = HorizontalAlignment.Center;
	}
}

