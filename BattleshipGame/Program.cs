using System;

namespace BattleshipGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var shotValidator = new ShotValidator(10);
            var board = new Board(10);
            board.PrepareNewGame(5, 4, 4);
            var boardPrinter = new BoardPrinter(board);
            boardPrinter.PrintBoard();

            while (true)
            {
                Console.WriteLine("Enter your shot, e.g A3:");
                string shot = Console.ReadLine();
                if (!shotValidator.IsValid(shot))
                {
                    Console.WriteLine("Invalid shot format.");
                    continue;
                }
                board.ApplyShot(shot);
                boardPrinter.PrintBoard();

                if (board.IsGameEnded())
                {
                    Console.WriteLine("You won. Press enter to exit.");
                    break;
                }
            }

            Console.ReadLine();
        }
    }
}
