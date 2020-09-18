using BattleshipGame;
using BattleshipGame.Exceptions;
using System;
using System.Linq;
using Xunit;

namespace BattleshipGameTests
{
    public class BoardTests
    {
        [Fact]
        public void ShouldHaveCorrectGridSizes()
        {
            var board = new Board(10);
            board.PrepareNewGame(10);
            Assert.Equal(10, board.GetUserGrid().GetLength(0));
            Assert.Equal(10, board.GetUserGrid().GetLength(1));
            Assert.Equal(10, board.GetComputerGrid().GetLength(0));
            Assert.Equal(10, board.GetComputerGrid().GetLength(0));
        }

        [Fact]
        public void ShouldHaveSomeIsShipSquaresAfterPreparation()
        {
            var board = new Board(10);
            board.PrepareNewGame(10);
            var grid = board.GetComputerGrid();
            var haveAnyShip = false;
            for (var r = 0; r < grid.GetLength(0); r++)
                for (var c = 0; c < grid.GetLength(1); c++)
                    if (grid[r, c] == GameSquare.IsShip)
                    {
                        haveAnyShip = true;
                    }
            Assert.True(haveAnyShip);
        }
        [Fact]
        public void ShouldHaveNoIsShipSquaresAfterPreparation()
        {
            var board = new Board(10);
            board.PrepareNewGame();
            var grid = board.GetComputerGrid();
            var haveAnyShip = false;
            for (var r = 0; r < grid.GetLength(0); r++)
                for (var c = 0; c < grid.GetLength(1); c++)
                    if (grid[r, c] == GameSquare.IsShip)
                    {
                        haveAnyShip = true;
                    }
            Assert.False(haveAnyShip);
        }

        [Fact]
        public void ShouldReturnGameEndedIfAllSquaresChecked()
        {
            var board = new Board(10);
            board.PrepareNewGame(5, 4, 4);
            var columns = Enumerable.Range('A', 10).Select(x => ((char)x).ToString()).ToArray();
            var rows = Enumerable.Range(1, 10).Select(x => x.ToString()).ToArray();
            foreach (var c in columns)
                foreach (var r in rows)
                    board.ApplyShot($"{c}{r}");
            Assert.True(board.IsGameEnded());
        }
        [Fact]
        public void ShouldReturnGameNotEndedIfNoSquaresChecked()
        {
            var board = new Board(10);
            board.PrepareNewGame(5, 4, 4);
            Assert.False(board.IsGameEnded());
        }


        [Fact]
        public void ShouldTrhowExceptionIfBattleShipCannotBeAdded()
        {
            var board = new Board(10);
            Assert.Throws<BoardException>(() => board.PrepareNewGame(5, 11));
           
        }



    }
}
