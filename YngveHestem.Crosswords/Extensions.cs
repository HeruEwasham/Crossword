using System;
namespace YngveHestem.Crosswords
{
	public static class Extensions
	{
		public static string ToString(this char[,] chars, string delimeterColumn, string delimeterRow, bool delimeterColumnShouldStartBeforeFirstColumnInRow = false, bool delimeterRowShouldStartBeforeFirstRow = false, bool delimeterColumnShouldBeSetBeforeNewRow = false, bool delimeterRowShouldBeSetAfterLastRow = false)
		{
            var result = string.Empty;
            for (var i = 0; i < chars.GetLength(1); i++)
            {
                if (delimeterRowShouldStartBeforeFirstRow || i != 0)
                {
                    result += delimeterRow;
                }
                for (var j = 0; j < chars.GetLength(0); j++)
                {
                    if (delimeterColumnShouldStartBeforeFirstColumnInRow || i != 0)
                    {
                        result += delimeterColumn;
                    }
                    result += chars[j, i];
                }

                if (delimeterColumnShouldBeSetBeforeNewRow)
                {
                    result += delimeterColumn;
                }
            }

            if (delimeterRowShouldBeSetAfterLastRow)
            {
                result += delimeterRow;
            }
            return result;
        }
	}
}

