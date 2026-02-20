using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Graphics;

namespace YngveHestem.Crosswords
{
    public class Crossword
    {
        /// <summary>
        /// This character is used to define fields that are "black boxes".
        /// </summary>
        public const char BLACK_BOX_CHARACTER = '$';

        /// <summary>
        /// This character is used to define fields where a character should be filled in.
        /// </summary>
        public const char WHITE_BOX_CHARACTER = ' ';

        /// <summary>
        /// Get the board width.
        /// </summary>
        public int BoardWidth { get => _filledBoard.GetLength(0); }

        /// <summary>
        /// Get the board height.
        /// </summary>
        public int BoardHeight { get => _filledBoard.GetLength(1); }

        private readonly char[,] _filledBoard;
        private readonly List<WordWithInfo> _words;
        private readonly Dictionary<string, Tuple<int, int, Orientation>> _wordPlacement;

        private Crossword(char[,] filledBoard, List<WordWithInfo> words, Dictionary<string, Tuple<int, int, Orientation>> wordPlacement)
        {
            _filledBoard = filledBoard;
            _words = words;
            _wordPlacement = wordPlacement;
        }

        /// <summary>
        /// Check if a character is placed correct.
        /// </summary>
        /// <param name="input">The inputted character.</param>
        /// <param name="posX">The x position (column).</param>
        /// <param name="posY">The y position (row).</param>
        /// <returns></returns>
        public bool CheckCorrect(char input, int posX, int posY)
        {
            return _filledBoard[posX, posY] == char.ToUpper(input);
        }

        /// <summary>
        /// Get a list of the words in the crossword, including the position and orientation in the puzzle.
        /// </summary>
        /// <returns></returns>
        public List<WordWithInfoAndPlacement> GetWords()
        {
            var result = new List<WordWithInfoAndPlacement>();
            foreach(var word in _words)
            {
                result.Add(new WordWithInfoAndPlacement(word.Word, word.Hint, _wordPlacement[word.Word]));
            }
            return result;
        }

        /// <summary>
        /// Get a copy of the filled in board (the full board with "answers", and BLACK_BOX_CHARACTER in the "black boxes"). The first dimension is width/columns and the second is height/rows.
        /// </summary>
        /// <returns></returns>
        public char[,] GetFilledBoard()
        {
            return (char[,])_filledBoard.Clone();
        }

        /// <summary>
        /// Get a copy of the filled board where all the characters in the answers are replaced with the WHITE_BOX_CHARACTER. The rest are filled with BLACK_BOX_CHARACTER. The first dimension is width/columns and the second is height/rows.
        /// </summary>
        /// <returns></returns>
        public char[,] GetEmptyBoard()
        {
            var result = GetFilledBoard();
            for (var i = 0; i < result.GetLength(0); i++)
            {
                for (var j = 0; j < result.GetLength(1); j++)
                {
                    if (result[i, j] != BLACK_BOX_CHARACTER)
                    {
                        result[i, j] = WHITE_BOX_CHARACTER;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Get a board with answers printed as a string. The black boxes are set to the BLACK_BOX_CHARACTER.
        /// </summary>
        /// <returns></returns>
        public string GetFilledBoardAsString()
        {
            return _filledBoard.ToString(string.Empty, Environment.NewLine);
        }

        /// <summary>
        /// Get a board printed as a string, where all the characters in the answers are replaced with the WHITE_BOX_CHARACTER. The black boxes are set to the BLACK_BOX_CHARACTER.
        /// </summary>
        /// <returns></returns>
        public string GetEmptyBoardAsString()
        {
            return GetEmptyBoard().ToString(string.Empty, Environment.NewLine);
        }

        /// <summary>
        /// Get a board with answers printed as a string with "|" between each cell. The black boxes are set to the BLACK_BOX_CHARACTER.
        /// </summary>
        /// <returns></returns>
        public string GetFilledBoardAsTableString()
        {
            return _filledBoard.ToString("|", Environment.NewLine, true, false, true, false);
        }

        /// <summary>
        /// Get a board printed as a string with "|" between each cell, where all the characters in the answers are replaced with the WHITE_BOX_CHARACTER. The black boxes are set to the BLACK_BOX_CHARACTER.
        /// </summary>
        /// <returns></returns>
        public string GetEmptyBoardAsTableString()
        {
            return GetEmptyBoard().ToString("|", Environment.NewLine, true, false, true, false);
        }

        /// <summary>
        /// Fills in the correct characters in the given word.
        /// </summary>
        /// <param name="canvas">The canvas to draw in. The canvas should in most cases already have a board drawn in it.</param>
        /// <param name="word">The word to draw. This needs to be one of the words in the crossword.</param>
        /// <param name="charOptions">>The character options.</param>
        /// <param name="crosswordOptions">The crossword-options. This need to be the same settings as used when the board was drawn for the characters to draw correctly.</param>
        /// <returns>True if all characters has been drawn correctly and word exists.</returns>
        public bool DrawWord(ICanvas canvas, string word, DrawCharactersOptions charOptions, DrawCrosswordOptions crosswordOptions)
        {
            var wordToUpper = word.ToUpper();
            var wordWithInfo = GetWords().FirstOrDefault(w => w.Word == wordToUpper);

            if (wordWithInfo == null)
            {
                return false;
            }

            return DrawWord(canvas, wordWithInfo, charOptions, crosswordOptions);
        }

        /// <summary>
        /// Fills in the correct characters in the given word.
        /// </summary>
        /// <param name="canvas">The canvas to draw in. The canvas should in most cases already have a board drawn in it.</param>
        /// <param name="word">The word to draw. This needs to be one of the given words that can be returned from GetWords().</param>
        /// <param name="charOptions">>The character options.</param>
        /// <param name="crosswordOptions">The crossword-options. This need to be the same settings as used when the board was drawn for the characters to draw correctly.</param>
        /// <returns>True if all characters has been drawn correctly.</returns>
        public bool DrawWord(ICanvas canvas, WordWithInfoAndPlacement word, DrawCharactersOptions charOptions, DrawCrosswordOptions crosswordOptions)
        {
            if (word == null)
            {
                return false;
            }

            if (word.WordOrientation == Orientation.Vertical)
            {
                var nextPosY = word.PosY;
                for (var i = 0; i < word.Word.Length; i++)
                {
                    if (!DrawCorrectCharacterInCell(canvas, word.PosX, nextPosY, charOptions, crosswordOptions))
                    {
                        return false;
                    }
                    nextPosY++;
                }
            }
            else if (word.WordOrientation == Orientation.Horizontal)
            {
                var nextPosX = word.PosX;
                for (var i = 0; i < word.Word.Length; i++)
                {
                    if (!DrawCorrectCharacterInCell(canvas, nextPosX, word.PosY, charOptions, crosswordOptions))
                    {
                        return false;
                    }
                    nextPosX++;
                }
            }

            return true;
        }

        /// <summary>
        /// Fills in the the correct character at the given cell at posX/posY. If the given cell is not a character cell, it will just return false.
        /// </summary>
        /// <param name="canvas">The canvas to draw in. The canvas should in most cases already have a board drawn in it.</param>
        /// <param name="posX">The x-position of the cell.</param>
        /// <param name="posY">The y-position of the cell.</param>
        /// <param name="charOptions">The character options.</param>
        /// <param name="crosswordOptions">The crossword-options. This need to be the same settings as used when the board was drawn for the characters to draw correctly.</param>
        /// <returns>True if drawn and false if not drawn.</returns>
        public bool DrawCorrectCharacterInCell(ICanvas canvas, int posX, int posY, DrawCharactersOptions charOptions, DrawCrosswordOptions crosswordOptions)
        {
            return DrawGivenCharacterInCell(canvas, posX, posY, _filledBoard[posX, posY], charOptions, crosswordOptions);
        }

        /// <summary>
        /// Fills in the the specified character at the given cell at posX/posY. If the given cell is not a character cell, it will just return false.
        /// </summary>
        /// <param name="canvas">The canvas to draw in. The canvas should in most cases already have a board drawn in it.</param>
        /// <param name="posX">The x-position of the cell.</param>
        /// <param name="posY">The y-position of the cell.</param>
        /// <param name="character">The character to write.</param>
        /// <param name="charOptions">The character options.</param>
        /// <param name="crosswordOptions">The crossword-options. This need to be the same settings as used when the board was drawn for the characters to draw correctly.</param>
        /// <returns>True if drawn and false if not drawn.</returns>
        public bool DrawGivenCharacterInCell(ICanvas canvas, int posX, int posY, char character, DrawCharactersOptions charOptions, DrawCrosswordOptions crosswordOptions)
        {
            if (_filledBoard[posX, posY] == BLACK_BOX_CHARACTER)
            {
                return false;
            }

            var gridColumnWidth = CalculateCellSize(crosswordOptions.Width, crosswordOptions.LineThickness, BoardWidth);
            var gridColumnHeight = CalculateCellSize(crosswordOptions.Height, crosswordOptions.LineThickness, BoardHeight);
            var charPlacementX = charOptions.CharacterPlacementX.HasValue ? charOptions.CharacterPlacementX.Value : gridColumnWidth / 2;
            var startX = crosswordOptions.StartX + (gridColumnWidth * posX + crosswordOptions.LineThickness * (posX + 1) + charPlacementX);
            var startY = crosswordOptions.StartY + (gridColumnHeight * posY + crosswordOptions.LineThickness * (posY + 1) + gridColumnHeight - charOptions.CharacterPlacementY);

            canvas.Font = null;
            canvas.Font = charOptions.CharacterFont;
            canvas.FontColor = charOptions.CharacterColor;
            if (charOptions.CharacterFontSize > 0)
            {
                canvas.FontSize = charOptions.CharacterFontSize;
            }
            else
            {
                canvas.FontSize = gridColumnHeight;
            }
            canvas.DrawString(character.ToString(), startX, startY, charOptions.CharacterHorizontalAlignment);

            return true;
        }

        /// <summary>
        /// Fills in the the specified color at the given cell at posX/posY. If the given cell is not a character cell, it will just return false.
        /// </summary>
        /// <param name="canvas">The canvas to draw in. The canvas should in most cases already have a board drawn in it.</param>
        /// <param name="posX">The x-position of the cell.</param>
        /// <param name="posY">The y-position of the cell.</param>
        /// <param name="color">The color to use.</param>
        /// <param name="crosswordOptions">The crossword-options. This need to be the same settings as used when the board was drawn for the characters to draw correctly.</param>
        /// <param name="drawOnlyIfCharacterCell">If true, it will not draw if the given cell is a "BLACK BOX". Ie. Is a part of the background.</param>
        /// <returns>True if drawn and false if not drawn.</returns>
        public bool SetColorInCell(ICanvas canvas, int posX, int posY, Color color, DrawCrosswordOptions crosswordOptions, bool drawOnlyIfCharacterCell = true)
        {
            if (drawOnlyIfCharacterCell && _filledBoard[posX, posY] == BLACK_BOX_CHARACTER)
            {
                return false;
            }

            var gridColumnWidth = CalculateCellSize(crosswordOptions.Width, crosswordOptions.LineThickness, BoardWidth);
            var gridColumnHeight = CalculateCellSize(crosswordOptions.Height, crosswordOptions.LineThickness, BoardHeight);
            var startX = crosswordOptions.StartX + (gridColumnWidth * posX + crosswordOptions.LineThickness * (posX + 1));
            var startY = crosswordOptions.StartY + (gridColumnHeight * posY + crosswordOptions.LineThickness * (posY + 1) + gridColumnHeight);

            canvas.FillColor = color;
            canvas.FillRectangle(startX, startY, gridColumnWidth, -gridColumnHeight);

            return true;
        }

        /// <summary>
        /// From a pixel relative to the image/canvas, give the cell this is in. So if given a position based on a mouse click, it will return which cell that has been clicked.
        /// </summary>
        /// <param name="pixelPosX">The x position of the pixel relative to the image/canvas.</param>
        /// <param name="pixelPosY">The x position of the pixel relative to the image/canvas.</param>
        /// <param name="crosswordOptions">The crossword-options. This need to be the same settings as used when the board was drawn for the characters to draw correctly.</param>
        /// <param name="returnNullIfCellIsABackgroundCell">If true, it will not return the position if the cell clicked is a "BLACK BOX"-cell. A cell that is not a character cell. If false, it will return the cell anyway.</param>
        /// <returns></returns>
        public Tuple<int, int> GetCellPositionFromPixel(int pixelPosX, int pixelPosY, DrawCrosswordOptions crosswordOptions, bool returnNullIfCellIsABackgroundCell = false)
        {
            if (pixelPosX < crosswordOptions.StartX || pixelPosY < crosswordOptions.StartY)
            {
                return null;
            }

            var gridColumnWidth = CalculateCellSize(crosswordOptions.Width, crosswordOptions.LineThickness, BoardWidth);
            var gridColumnHeight = CalculateCellSize(crosswordOptions.Height, crosswordOptions.LineThickness, BoardHeight);
            var endX = crosswordOptions.StartX + crosswordOptions.Width;
            var endY = crosswordOptions.StartY + crosswordOptions.Height;
            
            if (pixelPosX > endX || pixelPosY > endY)
            {
                return null;
            }
            var cellXPos = (int)Math.Floor((pixelPosX - crosswordOptions.StartX) / gridColumnWidth);
            var cellYPos = (int)Math.Floor((pixelPosY - crosswordOptions.StartY) / gridColumnHeight);
            return new Tuple<int, int>(cellXPos, cellYPos);
        }

        /// <summary>
        /// Draws a cipher crossword. This means that each of the letters placed have a number associated with it in the corner. Each corresponding number has the same letter. The result returned contains which letter and number that corresponds.
        /// </summary>
        /// <param name="canvas">The canvas to draw in.</param>
        /// <param name="options">The options, including the width and height.</param>
        /// <exception cref="ArgumentException">Are any of the arguments so unreasonable that it will not be able to draw correctly at all (like the width or height are smaller than it can be).</exception>
        /// <returns>The result returned contains which letter and number that corresponds.</returns>
        public Dictionary<char, int> DrawCipherCrossword(ICanvas canvas, DrawNumbersCrosswordOptions options)
        {
            DrawCluelessCrossword(canvas, options);

            var res = new Dictionary<char, int>();
            canvas.Font = null;
            canvas.Font = options.NumberFont;
            canvas.FontColor = options.NumberFontColor;
            canvas.FontSize = options.NumberFontSize;
            var gridColumnWidth = CalculateCellSize(options.Width, options.LineThickness, BoardWidth);
            var gridColumnHeight = CalculateCellSize(options.Height, options.LineThickness, BoardHeight);

            var nextNumber = 1;
            for (var i = 0; i < _filledBoard.GetLength(0); i++)
            {
                var columnStartX = options.StartX + (gridColumnWidth * i + options.LineThickness * (i + 1) + options.NumberPlacementX);
                for (var j = 0; j < _filledBoard.GetLength(1); j++)
                {
                    var columnStartY = options.StartY + (gridColumnHeight * j + options.LineThickness * (j + 1) + options.NumberPlacementY + options.NumberFontSize);
                    if (_filledBoard[i, j] != BLACK_BOX_CHARACTER)
                    {
                        if (res.ContainsKey(_filledBoard[i, j]))
                        {
                            canvas.DrawString(res[_filledBoard[i, j]].ToString(), columnStartX + options.NumberPlacementX, columnStartY + options.NumberPlacementY, HorizontalAlignment.Left);
                        }
                        else
                        {
                            canvas.DrawString(nextNumber.ToString(), columnStartX + options.NumberPlacementX, columnStartY + options.NumberPlacementY, HorizontalAlignment.Left);
                            res.Add(_filledBoard[i, j], nextNumber);
                            nextNumber++;
                        }
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Draws a cryptic crossword. This means that each of the words placed have a number associated with it in the corner. The returned result will contain which numbers are which word in horizontal and vertical way.
        /// </summary>
        /// <param name="canvas">The canvas to draw in.</param>
        /// <param name="options">The options, including the width and height.</param>
        /// <exception cref="ArgumentException">Are any of the arguments so unreasonable that it will not be able to draw correctly at all (like the width or height are smaller than it can be).</exception>
        /// <returns>The words and number associated with them in both horizontal and vertical way.</returns>
        public CrypticCrosswordResult DrawCrypticCrossword(ICanvas canvas, DrawNumbersCrosswordOptions options)
        {
            DrawCluelessCrossword(canvas, options);

            canvas.Font = null;
            canvas.Font = options.NumberFont;
            canvas.FontColor = options.NumberFontColor;
            canvas.FontSize = options.NumberFontSize;
            var gridColumnWidth = CalculateCellSize(options.Width, options.LineThickness, BoardWidth);
            var gridColumnHeight = CalculateCellSize(options.Height, options.LineThickness, BoardHeight);
            var wordsByPlacement = GetWords().OrderBy(x => x.PosX).ThenBy(x => x.PosY).ToArray();
            var resultHorizontal = new Dictionary<int, WordWithInfoAndPlacement>();
            var resultVertical = new Dictionary<int, WordWithInfoAndPlacement>();

            var writtenNumber = 1;
            for (var i = 0; i < wordsByPlacement.Length; i++)
            {
                var columnStartX = options.StartX + (gridColumnWidth * wordsByPlacement[i].PosX + options.LineThickness * (wordsByPlacement[i].PosX + 1) + options.NumberPlacementX);
                var columnStartY = options.StartY + (gridColumnHeight * wordsByPlacement[i].PosY + options.LineThickness * (wordsByPlacement[i].PosY + 1) + options.NumberPlacementY + options.NumberFontSize);
                if (wordsByPlacement[i].WordOrientation == Orientation.Vertical)
                {
                    if (resultHorizontal.Any(p => p.Value.PosX == wordsByPlacement[i].PosX && p.Value.PosY == wordsByPlacement[i].PosY))
                    {
                        resultVertical.Add(resultHorizontal.First(p => p.Value.PosX == wordsByPlacement[i].PosX && p.Value.PosY == wordsByPlacement[i].PosY).Key, wordsByPlacement[i]);
                    }
                    else
                    {
                        resultVertical.Add(writtenNumber, wordsByPlacement[i]);
                        canvas.DrawString(writtenNumber.ToString(), columnStartX + options.NumberPlacementX, columnStartY + options.NumberPlacementY, HorizontalAlignment.Left);
                        writtenNumber++;
                    }
                }
                else if (wordsByPlacement[i].WordOrientation == Orientation.Horizontal)
                {
                    if (resultVertical.Any(p => p.Value.PosX == wordsByPlacement[i].PosX && p.Value.PosY == wordsByPlacement[i].PosY))
                    {
                        resultHorizontal.Add(resultVertical.First(p => p.Value.PosX == wordsByPlacement[i].PosX && p.Value.PosY == wordsByPlacement[i].PosY).Key, wordsByPlacement[i]);
                    }
                    else
                    {
                        resultHorizontal.Add(writtenNumber, wordsByPlacement[i]);
                        canvas.DrawString(writtenNumber.ToString(), columnStartX + options.NumberPlacementX, columnStartY + options.NumberPlacementY, HorizontalAlignment.Left);
                        writtenNumber++;
                    }
                }
            }

            return new CrypticCrosswordResult(resultHorizontal, resultVertical);
        }

        /// <summary>
        /// Draws a clueless crossword based on the given parameters. This only generates the board, and not any clues for which word is which.
        /// </summary>
        /// <param name="canvas">The canvas to draw in.</param>
        /// <param name="options">The options, including the width and height.</param>
        /// <exception cref="ArgumentException">Are any of the arguments so unreasonable that it will not be able to draw correctly at all (like the width or height are smaller than it can be).</exception>
        public void DrawCluelessCrossword(ICanvas canvas, DrawCrosswordOptions options)
        {
            var minAlowedWidth = BoardWidth + ((BoardWidth + 1) * options.LineThickness);
            var minAlowedHeight = BoardHeight + ((BoardHeight + 1) * options.LineThickness);
            if (options.Width < minAlowedWidth || options.Height < minAlowedHeight)
            {
                throw new ArgumentException("The height and/or width given are not big enough to draw the crossword with the current settigs. The minimum width needed would be " + minAlowedWidth + " and height " + minAlowedHeight + ". The crossword could not be drawn.");
            }

            // Draw the background (which becomes lines and empty spaces).
            canvas.FillColor = options.BackgroundColor;
            canvas.FillRectangle(options.StartX, options.StartY, options.Width, options.Height);

            // Get the width and height for the character boxes and setting up the start of drawing theese. Then draw them.
            var gridColumnWidth = CalculateCellSize(options.Width, options.LineThickness, BoardWidth);
            var gridColumnHeight = CalculateCellSize(options.Height, options.LineThickness, BoardHeight);
            var nextColumnStartWidth = options.StartX + options.LineThickness;
            var nextColumnStartHeight = options.StartY + options.LineThickness;
            canvas.FillColor = options.CharacterCellColor;
            for (var i = 0; i < BoardWidth; i++)
            {
                for (var j = 0; j < BoardHeight; j++)
                {
                    if (_filledBoard[i,j] != BLACK_BOX_CHARACTER)
                    {
                        canvas.FillRectangle(nextColumnStartWidth, nextColumnStartHeight, gridColumnWidth, gridColumnHeight);
                    }

                    if (j < BoardHeight-1)
                    {
                        nextColumnStartHeight += gridColumnHeight + options.LineThickness;
                    }
                    else
                    {
                        nextColumnStartHeight = options.StartY + options.LineThickness;
                    }
                }
                if (i < BoardWidth-1)
                {
                    nextColumnStartWidth += gridColumnWidth + options.LineThickness;
                }
                else
                {
                    nextColumnStartWidth = options.StartX + options.LineThickness;
                }
            }
        }

        private static float CalculateCellSize(float totalPixels, float lineThickness, int numberOfCells)
        {
            return ((totalPixels - lineThickness) / numberOfCells) - lineThickness;
        }

        public static Crossword Generate(CrosswordOptions options)
        {
            if (options.Words == null || options.Words.Count < 2)
            {
                throw new ArgumentException("You must provide at least two words you want in the crossword.");
            }
            if (options.Glossary == null)
            {
                throw new ArgumentException("You must provide a glossary.");
            }
            var board = GenerateEmptyBoard(options.BoardWidth, options.BoardHeight);

            var sortedWords = options.Words.OrderByDescending(w => w.Word.Length).Select(w => w.Word).ToArray();

            if (sortedWords[0].Length > board.GetLength(0) && sortedWords[0].Length > board.GetLength(1))
            {
                throw new ArgumentException("The words must be possible to place in at least one of the two dimensions.");
            }
            var filledBoard = PlaceWordsOnBoard(board, sortedWords, options.Glossary, out var wordPlacement);

            return new Crossword(filledBoard, options.Words.Where(w => wordPlacement.Keys.Contains(w.Word)).ToList(), wordPlacement);
        }

        private static char[,] PlaceWordsOnBoard(char[,] board, string[] sortedWords, List<WordWithInfo> glossary, out Dictionary<string, Tuple<int, int, Orientation>> wordIndex)
        {
            var tempBoard = (char[,])board.Clone();
            var wordsInQueue = new Queue<string>(sortedWords);
            var firstWord = wordsInQueue.Dequeue();
            wordIndex = new Dictionary<string, Tuple<int, int, Orientation>>
            {
                { firstWord, PlaceFirstWord(tempBoard, firstWord) }
            };
            var wordsNotPlacedSinceLastPlacement = 0;
            while (wordsInQueue.Count > 0 && wordsNotPlacedSinceLastPlacement < wordsInQueue.Count)
            {
                var word = wordsInQueue.Dequeue();
                var placed = PlaceWord(tempBoard, word, glossary, wordIndex);
                if (placed != null)
                {
                    wordIndex.Add(word, placed);
                    wordsNotPlacedSinceLastPlacement = 0;
                }
                else
                {
                    wordsInQueue.Enqueue(word);
                    wordsNotPlacedSinceLastPlacement++;
                }
            }

            return tempBoard;
        }

        private static Tuple<int, int, Orientation> PlaceWord(char[,] board, string word, List<WordWithInfo> glossary, Dictionary<string, Tuple<int, int, Orientation>> wordIndex)
        {
            var possibleCrossingPositions = FindPossibleCrossingPositions(board, word, wordIndex);
            var validStartPositions = ValidateCrossings(possibleCrossingPositions, board, word, glossary, wordIndex);
            if (validStartPositions.Count > 0)
            {
                var placement = validStartPositions.OrderByDescending(o => o.Item4).FirstOrDefault();
                PlaceWordOnBoard(board, word, placement.Item1, placement.Item2, placement.Item3);
                return new Tuple<int, int, Orientation>(placement.Item1, placement.Item2, placement.Item3);
            }
            else
            {
                return null;
            }
        }

        private static List<Tuple<int, int, Orientation, int>> ValidateCrossings(List<Tuple<int, int>> possibleCrossingPositions, char[,] board, string word, List<WordWithInfo> glossary, Dictionary<string, Tuple<int, int, Orientation>> wordIndex)
        {
            var result = new List<Tuple<int, int, Orientation, int>>();
            for(var i = 0; i < possibleCrossingPositions.Count; i++)
            {
                var character = board[possibleCrossingPositions[i].Item1, possibleCrossingPositions[i].Item2];
                var positionsOfCharacterInWord = new List<int>();
                for (var j = 0; j < word.Length; j++)
                {
                    if (word[j] == character)
                    {
                        positionsOfCharacterInWord.Add(j+1);    // +1 as this uses actual characters and not the programmatic ones.
                    }
                }

                for (var j = 0; j < positionsOfCharacterInWord.Count; j++)
                {
                    // Check if possible horizontally:
                    var posBeforeWord = possibleCrossingPositions[i].Item1 - positionsOfCharacterInWord[j];
                    var posAfterWord = possibleCrossingPositions[i].Item1 + (word.Length - positionsOfCharacterInWord[j]) + 1;
                    if (posBeforeWord > -2 &&                                                       // Start of word is not out of bounce.
                        posAfterWord < board.GetLength(0) + 1 &&                                    // End of word is not out of bounce.
                        (posBeforeWord == -1 || board[posBeforeWord, possibleCrossingPositions[i].Item2] == BLACK_BOX_CHARACTER) && // Start of word is either at the edge or word has a black box before it.
                        (posAfterWord == board.GetLength(0) || board[posAfterWord, possibleCrossingPositions[i].Item2] == BLACK_BOX_CHARACTER)) // End of word is either at the edge or word has a black box after it.
                    {
                        // Seems to be possible, check that possible adjecent words on both sides are valid
                        if (AdjecentWordsWork(board, word, posBeforeWord + 1, possibleCrossingPositions[i].Item2, glossary, Orientation.Horizontal, out var score))
                        {
                            result.Add(new Tuple<int, int, Orientation, int>(posBeforeWord + 1, possibleCrossingPositions[i].Item2, Orientation.Horizontal, score));
                        }
                    }

                    // Check if possible vertically:
                    posBeforeWord = possibleCrossingPositions[i].Item2 - positionsOfCharacterInWord[j];
                    posAfterWord = possibleCrossingPositions[i].Item2 + (word.Length - positionsOfCharacterInWord[j]) + 1;
                    if (posBeforeWord > -2 &&                                                       // Start of word is not out of bounce.
                        posAfterWord < board.GetLength(1) + 1 &&                                    // End of word is not out of bounce.
                        (posBeforeWord == -1 || board[possibleCrossingPositions[i].Item1, posBeforeWord] == BLACK_BOX_CHARACTER) && // Start of word is either at the edge or word has a black box before it.
                        (posAfterWord == board.GetLength(1) || board[possibleCrossingPositions[i].Item1, posAfterWord] == BLACK_BOX_CHARACTER)) // End of word is either at the edge or word has a black box after it.
                    {
                        // Seems to be possible, check that possible adjecent words on both sides are valid
                        if (AdjecentWordsWork(board, word, possibleCrossingPositions[i].Item1, posBeforeWord + 1, glossary, Orientation.Vertical, out var score))
                        {
                            result.Add(new Tuple<int, int, Orientation, int>(possibleCrossingPositions[i].Item1, posBeforeWord + 1, Orientation.Vertical, score));
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Check that all adjecent words work. This do also check that all characters in the words path is either empty or fits with the other characters.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="word"></param>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="glossary"></param>
        /// <param name="orientation"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        private static bool AdjecentWordsWork(char[,] board, string word, int startX, int startY, List<WordWithInfo> glossary, Orientation orientation, out int score)
        {
            score = 0;
            if (orientation == Orientation.Horizontal)
            {
                for (var i = 0; i < word.Length; i++)
                {
                    var currentXPos = startX + i;

                    // Be sure the specified character fit.
                    if (board[currentXPos, startY] != BLACK_BOX_CHARACTER && board[currentXPos, startY] != word[i])
                    {
                        return false;
                    }

                    // If new word is created, is it valid (if, set a higher score, else, return false)?
                    var newWord = GetWord(board, word[i], currentXPos, startY, Orientation.Vertical);
                    if (newWord.Length > 1)
                    {
                        if (glossary.Any(w => w.Word == newWord))
                        {
                            score += 1;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                for (var i = 0; i < word.Length; i++)
                {
                    var currentYPos = startY + i;

                    // Be sure the specified character fit.
                    if (board[startX, currentYPos] != BLACK_BOX_CHARACTER && board[startX, currentYPos] != word[i])
                    {
                        return false;
                    }

                    // If new word is created, is it valid (if, set a higher score, else, return false)?
                    var newWord = GetWord(board, word[i], startX, currentYPos, Orientation.Horizontal);
                    if (newWord.Length > 1)
                    {
                        if (glossary.Any(w => w.Word == newWord))
                        {
                            score += 1;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private static string GetWord(char[,] board, char character, int posX, int posY, Orientation orientation)
        {
            if (orientation == Orientation.Horizontal)
            {
                var wordStart = posX;
                var word = string.Empty;
                while (wordStart - 1 >= 0 && board[wordStart - 1, posY] != BLACK_BOX_CHARACTER)
                {
                    wordStart -= 1;
                }
                var currentPos = wordStart;
                while (currentPos < board.GetLength(0) && (board[currentPos, posY] != BLACK_BOX_CHARACTER || currentPos == posX))
                {
                    if (board[currentPos, posY] != BLACK_BOX_CHARACTER)
                    {
                        word += board[currentPos, posY];
                    }
                    else
                    {
                        word += character;
                    }
                    currentPos++;
                }
                return word;
            }
            else
            {
                var wordStart = posY;
                var word = string.Empty;
                while (wordStart - 1 >= 0 && board[posX, wordStart - 1] != BLACK_BOX_CHARACTER)
                {
                    wordStart -= 1;
                }
                var currentPos = wordStart;
                while (currentPos < board.GetLength(1) && (board[posX, currentPos] != BLACK_BOX_CHARACTER || currentPos == posY))
                {
                    if (board[posX, currentPos] != BLACK_BOX_CHARACTER)
                    {
                        word += board[posX, currentPos];
                    }
                    else
                    {
                        word += character;
                    }
                    currentPos++;
                }
                return word;
            }
        }

        private static List<Tuple<int, int>> FindPossibleCrossingPositions(char[,] board, string word, Dictionary<string, Tuple<int, int, Orientation>> wordIndex)
        {
            var result = new List<Tuple<int, int>>();
            var hasMatchingCharacters = word.Any(c => wordIndex.Keys.Any(k => k.Any(kc => kc == c)));
            if (!hasMatchingCharacters)
            {
                return result;
            }

            for (var i = 0; i < board.GetLength(0); i++)
            {
                for (var j = 0; j < board.GetLength(1); j++)
                {
                    // Check if crossing
                    if (word.Contains(board[i,j]))
                    {
                        result.Add(new Tuple<int, int>(i, j));
                    }
                }
            }

            return result;
        }

        private static Tuple<int, int, Orientation> PlaceFirstWord(char[,] board, string word)
        {
            var rand = new Random();
            var maxStartValueX = board.GetLength(0) - word.Length;
            var maxStartValueY = board.GetLength(1);
            var orientation = Orientation.Horizontal;
            if (maxStartValueX < 0)
            {
                maxStartValueX = board.GetLength(0);
                maxStartValueY = board.GetLength(1) - word.Length;
                orientation = Orientation.Vertical;
            }
            var randX = rand.Next(0, maxStartValueX);
            var randY = rand.Next(0, maxStartValueY);
            PlaceWordOnBoard(board, word, randX, randY, orientation);
            return new Tuple<int, int, Orientation>(randX, randY, orientation);
        }

        private static void PlaceWordOnBoard(char[,] board, string word, int startX, int startY, Orientation orientation)
        {
            if (orientation == Orientation.Vertical)
            {
                for (var i = 0; i < word.Length; i++)
                {
                    board[startX, startY + i] = word[i];
                }
            }
            else
            {
                for (var i = 0; i < word.Length; i++)
                {
                    board[startX + i, startY] = word[i];
                }
            }
        }

        private static char[,] GenerateEmptyBoard(int width, int height)
        {
            var board = new char[width, height];
            for(var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    board[i,j] = BLACK_BOX_CHARACTER;
                }
            }

            return board;
        }
    }
}

