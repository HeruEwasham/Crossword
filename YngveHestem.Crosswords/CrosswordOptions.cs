using System.Collections.Generic;

namespace YngveHestem.Crosswords
{
    public class CrosswordOptions
    {
        /// <summary>
        /// The number of rows (cells in vertical direction).
        /// </summary>
        public int BoardHeight = 10;

        /// <summary>
        /// The number of columns (cells in horizontal direction).
        /// </summary>
        public int BoardWidth = 10;

        /// <summary>
        /// The words you want to place on the board. The generator will try to use as many as it can, but based on the words it may not be able to fit all.
        /// </summary>
        public List<WordWithInfo> Words;

        /// <summary>
        /// A glossary of words. This glossary need to at least contain all the words that should be placed on the board. The generator uses this to be sure all words it generates are legal. Importing words from a dictionary in a given language would be a good suggestion.
        /// </summary>
        public List<WordWithInfo> Glossary;
    }
}