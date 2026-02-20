using System.Collections.Generic;

namespace YngveHestem.Crosswords
{
    public class CrypticCrosswordResult
    {
        /// <summary>
        /// The number and the word in horizontal direction.
        /// </summary>
        public Dictionary<int, WordWithInfoAndPlacement> HorizontalWords { get; }

        /// <summary>
        /// The number and the word in vertical direction.
        /// </summary>
        public Dictionary<int, WordWithInfoAndPlacement> VerticalWords { get; }

        public CrypticCrosswordResult(Dictionary<int, WordWithInfoAndPlacement> horizontalWords, Dictionary<int, WordWithInfoAndPlacement> verticalWords)
        {
            HorizontalWords = horizontalWords;
            VerticalWords = verticalWords;
        }
    }
}