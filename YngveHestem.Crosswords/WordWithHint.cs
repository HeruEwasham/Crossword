namespace YngveHestem.Crosswords
{
    public class WordWithHint
    {
        /// <summary>
        /// The word.
        /// </summary>
        public string Word { get; }

        /// <summary>
        /// A hint about what the word is. This may or may not be set.
        /// </summary>
        public string Hint { get; }

        public WordWithHint(string word, string hint)
        {
            Word = word.ToUpper();
            Hint = hint;
        }
    }
}