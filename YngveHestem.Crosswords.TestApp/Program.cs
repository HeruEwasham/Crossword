using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;

namespace YngveHestem.Crosswords.TestApp;

class Program
{
    static void Main(string[] args)
    {
        var words = new List<WordWithInfo>
        {
            new WordWithInfo("bord", "A furniture"),
            new WordWithInfo("sofa", "Something to sit on"),
            new WordWithInfo("seng", "Somewhere to sleep"),
            new WordWithInfo("frokost", "A meal"),
            new WordWithInfo("Ny", "A train-company"),
            new WordWithInfo("Vinter", "A season"),
            new WordWithInfo("Vin", "A form of drink"),
            new WordWithInfo("Entrecote", "Meat"),
            new WordWithInfo("singel", "Alone"),
            new WordWithInfo("Revy", "A name"),
            new WordWithInfo("fylling", "Full"),
            new WordWithInfo("full", "Doing something"),
            new WordWithInfo("Urfolk", "Indianer"),
            new WordWithInfo("folk", "People"),
            new WordWithInfo("tidning", "Wheater"),
            new WordWithInfo("transport", "taxi"),
            new WordWithInfo("taxi", "transport"),
            new WordWithInfo("liste", "Writing"),
            new WordWithInfo("vertikal", "Orientation"),
            new WordWithInfo("Rist", "t"),
            new WordWithInfo("Vinne", "Beating"),
            new WordWithInfo("amalgamplombe", "Tannlege")
        };
        var glossary = CreateGlossaryFromLineSeparatedFile("/Users/yngvehestem/Downloads/nsf2022.txt");
        var crossword = Crossword.Generate(new CrosswordOptions
        {
            Words = words,
            Glossary = glossary,
            BoardHeight = 13,
            BoardWidth = 13
        });
        var resWords = crossword.GetWords();

        Console.WriteLine("Crossword:");
        Console.WriteLine(crossword.GetEmptyBoardAsTableString());
        Console.WriteLine(Environment.NewLine + "Answer:");
        Console.WriteLine(crossword.GetFilledBoardAsTableString());

        Console.WriteLine(Environment.NewLine + Environment.NewLine + "Words used: " + crossword.GetWords().Count + "/" + words.Count);

        using (var bmpClueless = new SkiaBitmapExportContext(700, 700, 1.0f))
        {
            bmpClueless.Canvas.FillColor = Colors.LightBlue;
            bmpClueless.Canvas.FillRectangle(0, 0, bmpClueless.Width, bmpClueless.Height);
            var options = new DrawCrosswordOptions
            {
                Width = bmpClueless.Width - 100,
                Height = bmpClueless.Height - 100,
                StartX = 50,
                StartY = 50
            };
            crossword.DrawCluelessCrossword(bmpClueless.Canvas, options);
            bmpClueless.WriteToFile("testClueless.png");
        }

        using (var bmpCryptic = new SkiaBitmapExportContext(700, 700, 1.0f))
        {
            bmpCryptic.Canvas.FillColor = Colors.LightBlue;
            bmpCryptic.Canvas.FillRectangle(0, 0, bmpCryptic.Width, bmpCryptic.Height);
            var options = new DrawNumbersCrosswordOptions
            {
                Width = bmpCryptic.Width - 100,
                Height = bmpCryptic.Height - 100,
                StartX = 50,
                StartY = 50
            };
            crossword.DrawCrypticCrossword(bmpCryptic.Canvas, options);
            bmpCryptic.WriteToFile("testCryptic.png");
        }

        using (var bmpCipher = new SkiaBitmapExportContext(700, 700, 1.0f))
        {
            bmpCipher.Canvas.FillColor = Colors.LightBlue;
            bmpCipher.Canvas.FillRectangle(0, 0, bmpCipher.Width, bmpCipher.Height);
            var options = new DrawNumbersCrosswordOptions
            {
                Width = bmpCipher.Width - 100,
                Height = bmpCipher.Height - 100,
                StartX = 50,
                StartY = 50
            };
            var res = crossword.DrawCipherCrossword(bmpCipher.Canvas, options);
            bmpCipher.WriteToFile("testCipher.png");

            var t = res.Select(x => x.Value + " = " + x.Key).ToArray();
            Console.WriteLine("Result:" + Environment.NewLine + string.Join(Environment.NewLine, t));
        }

        using (var bmp = new SkiaBitmapExportContext(500, 500, 1.0f))
        {
            var options = new DrawNumbersCrosswordOptions
            {
                Width = bmp.Width - 100,
                Height = bmp.Height - 100,
                StartX = 50,
                StartY = 50
            };
            var charOptions = new DrawCharactersOptions
            {
                CharacterColor = Colors.Red
            };
            bmp.Canvas.FillColor = Colors.LightBlue;
            bmp.Canvas.FillRectangle(0, 0, bmp.Width, bmp.Height);
            crossword.DrawCrypticCrossword(bmp.Canvas, options);
            crossword.DrawCorrectCharacterInCell(bmp.Canvas, resWords[0].PosX, resWords[0].PosY, charOptions, options);
            crossword.DrawCorrectCharacterInCell(bmp.Canvas, resWords[1].PosX, resWords[1].PosY, charOptions, options);
            crossword.DrawCorrectCharacterInCell(bmp.Canvas, resWords[2].PosX, resWords[2].PosY, charOptions, options);
            bmp.WriteToFile("testCryptic_characters.png");
        }

        using (var bmp = new SkiaBitmapExportContext(500, 500, 1.0f))
        {
            var options = new DrawNumbersCrosswordOptions
            {
                Width = bmp.Width - 100,
                Height = bmp.Height - 100,
                StartX = 50,
                StartY = 50
            };
            var charOptions = new DrawCharactersOptions
            {
                CharacterColor = Colors.Red
            };
            bmp.Canvas.FillColor = Colors.LightBlue;
            bmp.Canvas.FillRectangle(0, 0, bmp.Width, bmp.Height);
            crossword.DrawCrypticCrossword(bmp.Canvas, options);
            crossword.DrawWord(bmp.Canvas, "amalgamplombe", charOptions, options);
            bmp.WriteToFile("testCryptic_word.png");
        }

        using (var bmp = new SkiaBitmapExportContext(500, 500, 1.0f))
        {
            var options = new DrawNumbersCrosswordOptions
            {
                Width = bmp.Width - 100,
                Height = bmp.Height - 100,
                StartX = 50,
                StartY = 50
            };
            var charOptions = new DrawCharactersOptions
            {
                CharacterColor = Colors.Red
            };
            bmp.Canvas.FillColor = Colors.LightBlue;
            bmp.Canvas.FillRectangle(0, 0, bmp.Width, bmp.Height);
            crossword.DrawCipherCrossword(bmp.Canvas, options);
            crossword.DrawWord(bmp.Canvas, resWords[0], charOptions, options);
            bmp.WriteToFile("testCipher_word.png");
        }

        using (var bmp = new SkiaBitmapExportContext(500, 500, 1.0f))
        {
            var options = new DrawNumbersCrosswordOptions
            {
                Width = bmp.Width - 100,
                Height = bmp.Height - 100,
                StartX = 50,
                StartY = 50
            };
            var charOptions = new DrawCharactersOptions
            {
                CharacterColor = Colors.Red
            };
            bmp.Canvas.FillColor = Colors.LightBlue;
            bmp.Canvas.FillRectangle(0, 0, bmp.Width, bmp.Height);
            crossword.DrawCipherCrossword(bmp.Canvas, options);
            crossword.SetColorInCell(bmp.Canvas, resWords[0].PosX, resWords[0].PosY, Colors.Green, options);
            crossword.DrawWord(bmp.Canvas, resWords[0], charOptions, options);
            bmp.WriteToFile("testCipher_word_with_marked_first_character_in_word.png");
        }

        using (var bmp = new SkiaBitmapExportContext(500, 500, 1.0f))
        {
            var options = new DrawNumbersCrosswordOptions
            {
                Width = bmp.Width - 100,
                Height = bmp.Height - 100,
                StartX = 50,
                StartY = 50
            };
            var charOptions = new DrawCharactersOptions
            {
                CharacterColor = Colors.Red
            };
            bmp.Canvas.FillColor = Colors.LightBlue;
            bmp.Canvas.FillRectangle(0, 0, bmp.Width, bmp.Height);
            crossword.DrawCipherCrossword(bmp.Canvas, options);
            crossword.SetColorInCell(bmp.Canvas, resWords[0].PosX, resWords[0].PosY, Colors.Green, options);
            bmp.WriteToFile("testCipher_word_with_marked_cell.png");
        }

        using (var bmp = new SkiaBitmapExportContext(500, 500, 1.0f))
        {
            var options = new DrawNumbersCrosswordOptions
            {
                Width = bmp.Width - 100,
                Height = bmp.Height - 100,
                StartX = 50,
                StartY = 50
            };
            var charOptions = new DrawCharactersOptions
            {
                CharacterColor = Colors.Red
            };
            bmp.Canvas.FillColor = Colors.LightBlue;
            bmp.Canvas.FillRectangle(0, 0, bmp.Width, bmp.Height);
            crossword.SetColorInCell(bmp.Canvas, resWords[0].PosX, resWords[0].PosY, Colors.Green, options);
            crossword.DrawCipherCrossword(bmp.Canvas, options);
            crossword.DrawWord(bmp.Canvas, resWords[0], charOptions, options);
            bmp.WriteToFile("testCipher_word_with_marked_first_character_in_word_first.png");

            Console.WriteLine(Environment.NewLine + "-------- Pixel-click --------" + Environment.NewLine);
            Console.WriteLine("Start of board in pixels (width): " + options.StartX);
            Console.WriteLine("Start of board in pixels (height): " + options.StartY);
            Console.WriteLine("BoardWidth in pixels: " + options.Width);
            Console.WriteLine("BoardHeight in pixels: " + options.Height);
            Console.WriteLine("Number of cells in x-position: " + crossword.BoardWidth);
            Console.WriteLine("Number of cells in y-position: " + crossword.BoardHeight);
            var cellSizeX = ((options.Width - options.LineThickness) / crossword.BoardWidth) - options.LineThickness;
            var cellSizeY = ((options.Height - options.LineThickness) / crossword.BoardHeight) - options.LineThickness;
            Console.WriteLine("Cell size in x-position: " + cellSizeX);
            Console.WriteLine("Cell size in y-position: " + cellSizeY);
            for(var i = 0; i < crossword.BoardWidth; i++)
            {
                Console.WriteLine();
                var pixelPosXLeft = (int)Math.Floor(options.StartX + (cellSizeX * i) + (cellSizeX/3));
                var pixelPosXMiddle = (int)Math.Floor(options.StartX + (cellSizeX * i) + (cellSizeX/2));
                var pixelPosXRight = (int)Math.Floor(options.StartX + (cellSizeX * i) + (cellSizeX/1.5));
                for(var j = 0; j < crossword.BoardHeight; j++)
                {
                    int pixelPosYLeft = (int)Math.Floor(options.StartY + (cellSizeY * j) + (cellSizeY/3));
                    int pixelPosYMiddle = (int)Math.Floor(options.StartY + (cellSizeY * j) + (cellSizeY/2));
                    int pixelPosYRight = (int)Math.Floor(options.StartY + (cellSizeY * j) + (cellSizeY/1.5));
                    var cellLeftPos = crossword.GetCellPositionFromPixel(pixelPosXLeft, pixelPosYLeft, options);
                    var cellMiddlePos = crossword.GetCellPositionFromPixel(pixelPosXMiddle, pixelPosYMiddle, options);
                    var cellRightPos = crossword.GetCellPositionFromPixel(pixelPosXRight, pixelPosYRight, options);
                    if (cellLeftPos != null)
                    {
                        Console.WriteLine("Pixel " + pixelPosXLeft + "*" + pixelPosYLeft + ": Column " + cellLeftPos.Item1 + ", Row " + cellLeftPos.Item2);
                    }
                    else
                    {
                        Console.WriteLine("Pixel " + pixelPosXLeft + "*" + pixelPosYLeft + ": Outside crossword");
                    }
                    if (cellMiddlePos != null)
                    {
                        Console.WriteLine("Pixel " + pixelPosXMiddle + "*" + pixelPosYMiddle + ": Column " + cellMiddlePos.Item1 + ", Row " + cellMiddlePos.Item2);
                    }
                    else
                    {
                        Console.WriteLine("Pixel " + pixelPosXMiddle + "*" + pixelPosYMiddle + ": Outside crossword");
                    }
                    if (cellRightPos != null)
                    {
                        Console.WriteLine("Pixel " + pixelPosXRight + "*" + pixelPosYRight + ": Column " + cellRightPos.Item1 + ", Row " + cellRightPos.Item2);
                    }
                    else
                    {
                        Console.WriteLine("Pixel " + pixelPosXRight + "*" + pixelPosYRight + ": Outside crossword");
                    }
                }
            }
        }

        /*Seems like dpi is not supported by Skia.
         * var bmpCrypticDpi = new SkiaBitmapExportContext(700, 700, 1.0f, 300);
        bmpCrypticDpi.Canvas.FillColor = Colors.LightBlue;
        bmpCrypticDpi.Canvas.FillRectangle(0, 0, bmpCrypticDpi.Width, bmpCrypticDpi.Height);
        crossword.DrawCrypticCrossword(bmpCrypticDpi.Canvas, new DrawCrypticCrosswordOptions
        {
            Width = bmpCrypticDpi.Width - 100,
            Height = bmpCrypticDpi.Height - 100,
            StartX = 50,
            StartY = 50
        });
        bmpCrypticDpi.WriteToFile("testCryptic300Dpi.png");*/

        Console.ReadLine();
    }

    private static List<WordWithInfo> CreateGlossaryFromLineSeparatedFile(string filePath)
    {
        var result = new List<WordWithInfo>();
        var content = File.ReadAllLines(filePath);
        foreach(var line in content)
        {
            result.Add(new WordWithInfo(line.Trim(), string.Empty));
        }
        return result;
    }
}

