using System.Collections.Generic;

namespace YngveHestem.Crosswords
{
    public class CrypticCrosswordResult
    {
        /// <summary>
        /// The number and the word in horizontal direction.
        /// </summary>
        public Dictionary<int, WordWithHintAndPlacement> HorizontalWords { get; }

        /// <summary>
        /// The number and the word in vertical direction.
        /// </summary>
        public Dictionary<int, WordWithHintAndPlacement> VerticalWords { get; }

        public CrypticCrosswordResult(Dictionary<int, WordWithHintAndPlacement> horizontalWords, Dictionary<int, WordWithHintAndPlacement> verticalWords)
        {
            HorizontalWords = horizontalWords;
            VerticalWords = verticalWords;
        }
    }
}