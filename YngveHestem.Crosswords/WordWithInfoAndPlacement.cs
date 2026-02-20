using System;
namespace YngveHestem.Crosswords
{
	public class WordWithInfoAndPlacement : WordWithInfo
	{
		/// <summary>
		/// The X-position (column) of the first letter.
		/// </summary>
		public int PosX { get; }

		/// <summary>
		/// The Y-position (row) of the first letter.
		/// </summary>
		public int PosY { get; }

		/// <summary>
		/// Is the word horizontal (placed in a row from left to right), or vertical (placed in a column downwards)
		/// </summary>
		public Orientation WordOrientation { get; }

		internal WordWithInfoAndPlacement(string word, string hint, Tuple<int, int, Orientation> placementInfo) : base(word, hint)
		{
			PosX = placementInfo.Item1;
			PosY = placementInfo.Item2;
			WordOrientation = placementInfo.Item3;
		}
	}
}

