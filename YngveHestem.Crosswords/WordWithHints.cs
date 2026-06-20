using System;
using System.Collections.Generic;
using System.Linq;

namespace YngveHestem.Crosswords
{
    public class WordWithHints
    {
        /// <summary>
        /// The word.
        /// </summary>
        public string Word { get; }

        /// <summary>
        /// A hint about what the word is. This may or may not be set.
        /// </summary>
        public string[] Hints { get; }

        /// <summary>
        /// Create an entry with a word and multiple hints.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="hints"></param>
        public WordWithHints(string word, IEnumerable<string> hints)
        {
            Word = word.ToUpper();
            Hints = hints.ToArray();
        }

        /// <summary>
        /// Create an entry with a word and only one hint.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="hint"></param>
        public WordWithHints(string word, string hint)
        {
            Word = word.ToUpper();
            Hints = new string[] {hint};
        }

        /// <summary>
        /// Converts to a word with only one hint. The hint is selected randomly from the list of hints.
        /// </summary>
        /// <returns></returns>
        public WordWithHint ToWordWithHint()
        {
            string selectedHint = Hints[Extensions.Random.Value.Next(Hints.Length)];
            return new WordWithHint(Word, selectedHint);
        }
    }
}
