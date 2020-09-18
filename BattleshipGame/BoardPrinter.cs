using System;
using System.Linq;

namespace BattleshipGame
{
    class BoardPrinter
    {
        private readonly int _boardWidth;
        private readonly int _boardSize;
        private readonly Board _board;
        private string[] _columns;
        private string[] _rows;
        public BoardPrinter(Board board, int boardWidth = 73)
        {
            _board = board;
            _boardWidth = boardWidth;
            _boardSize = _board.GetBoardSize();
            _columns = Enumerable.Range('A', _boardSize).Select(x => ((char)x).ToString()).ToArray();
            _rows = Enumerable.Range(1, _boardSize).Select(x => x.ToString()).ToArray();
        }
        public void PrintBoard()
        {
            var grid = _board.GetUserGrid();
            Console.Clear();
            PrintRow(_columns, string.Empty);
            for (int i = 0; i < _boardSize; i++)
            {
                string[] row = new string[10];
                for (int y = 0; y < _boardSize; y++)
                {
                    row[y] = grid[i, y] switch
                    {
                        GameSquare.Empty => string.Empty,
                        GameSquare.BlindShoot => "x",
                        GameSquare.IsShip => "o",
                        GameSquare.IsSink => "[o]"
                    };
                }
                PrintRow(row, _rows[i]);
            }
        }
        private void PrintRow(string[] columns, string rowLabel)
        {
            int width = (_boardWidth - columns.Length + 1) / columns.Length + 1;
            string row = "|";
            row += AlignCentre(rowLabel, width) + "|";
            foreach (var column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        private string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }

    }
}
