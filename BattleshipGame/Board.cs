using BattleshipGame.Exceptions;
using System;

namespace BattleshipGame
{
    public class Board
    {
        private GameSquare[,] _userGrid;
        private GameSquare[,] _computerGrid;
        private readonly int _boardSize;
        private readonly ShotParser _shotParser = new ShotParser();
        public Board(int boardSize)
        {
            _boardSize = boardSize;
            _userGrid = new GameSquare[10, 10];
            _computerGrid = new GameSquare[10, 10];
        }

        public void PrepareNewGame(params int[] battleships)
        {
            _resetGrid(_userGrid);
            _resetGrid(_computerGrid);
            foreach (var battleshipSize in battleships)
                AddBattleShip(battleshipSize);
        }

        public void ApplyShot(string shot)
        {
            (var column, var row) = _shotParser.ParseShot(shot);
            column--; row--;
            _userGrid[row, column] = _computerGrid[row, column] switch
            {
                GameSquare.Empty => GameSquare.BlindShoot,
                GameSquare.IsShip => GameSquare.IsShip,
                GameSquare.IsHit => GameSquare.IsShip
            };
            if (_computerGrid[row, column] == GameSquare.IsShip)
            {
                _computerGrid[row, column] = GameSquare.IsHit;
            }
        }

        public bool IsGameEnded()
        {
            for (var r = 0; r < _computerGrid.GetLength(0); r++)
                for (var c = 0; c < _computerGrid.GetLength(1); c++)
                    if (_computerGrid[r, c] == GameSquare.IsShip)
                        return false;
            return true;
        }

        public int GetBoardSize()
        {
            return _boardSize;
        }
        public GameSquare[,] GetUserGrid()
        {
            return _userGrid;
        }

        public GameSquare[,] GetComputerGrid()
        {
            return _computerGrid;
        }

        private void AddBattleShip(int size)
        {
            int maxTryCount = 100000;
            while (maxTryCount-- > 0) {
                if (TryAddBattleShip(size))
                    return;
            }
            throw new BoardException("Battleship cannot be added");
        }

        private bool TryAddBattleShip(int size)
        {
            Random rnd = new Random();
            var column = rnd.Next(0, _boardSize);
            var row = rnd.Next(0, _boardSize);
            var direction = (Direction)rnd.Next(0, 2);
            if (IsBattleShipCanBeAdded(size, column, row, direction))
            {
                PutBattleShipToGrid(size, column, row, direction);
                return true;
            }
            return false;
        }

        private void PutBattleShipToGrid(int size, int column, int row, Direction direction)
        {
            var i = 0;
            while (i++ < size)
            {
                _computerGrid[row, column] = GameSquare.IsShip;
                if (direction == Direction.Horizontal)
                {
                    row++;
                }
                else
                {
                    column++;
                }
            }
        }
        private bool IsBattleShipCanBeAdded(int size, int column, int row, Direction direction)
        {
            var i = 0;
            while (i++ < size)
            {
                if (row < 0 || row >= _boardSize || column < 0 || column >= _boardSize)
                    return false;
               
                if (_computerGrid[row, column] == GameSquare.IsShip)
                    return false;

                for (var r = -1; r <= 1; r++)
                {
                    for(var c = -1; c <=1; c++)
                    {
                        var tRow = _fixGridIndex(row + r);
                        var tCol = _fixGridIndex(column + c);
                       
                        if (_computerGrid[tRow, tCol] == GameSquare.IsShip)
                            return false;
                    }
                }
                _ = direction == Direction.Horizontal ? row++ : column++;
            }
            return true;
        }

        private int _fixGridIndex(int index)
        {
            if (index < 0)
                index = 0;
            if (index >= _boardSize)
                index = _boardSize - 1;
            return index;
        }

        private void _resetGrid(GameSquare[,] grid)
        {
            for (var r = 0; r < grid.GetLength(0); r++)
                for (var c = 0; c < grid.GetLength(1); c++)
                    grid[r, c] = GameSquare.Empty;
        }
    }
}
