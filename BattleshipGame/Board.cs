using BattleshipGame.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

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
            _userGrid = new GameSquare[_boardSize, _boardSize];
            _computerGrid = new GameSquare[_boardSize, _boardSize];
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
            
            if (_computerGrid[row, column] == GameSquare.IsShip)
                _computerGrid[row, column] = GameSquare.IsHit;

            (var isSinked, var squares) = CheckIfIsSinked(row, column);
            if (isSinked)
            {
                squares.ToList().ForEach(i => {
                    _computerGrid[i.row, i.column] = GameSquare.IsSink;
                    _userGrid[i.row, i.column] = GameSquare.IsSink;
                });
            }

            _userGrid[row, column] = _computerGrid[row, column] switch
            {
                GameSquare.Empty => GameSquare.BlindShoot,
                GameSquare.IsShip => GameSquare.IsShip,
                GameSquare.IsHit => GameSquare.IsShip,
                GameSquare.IsSink => GameSquare.IsSink
            };

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
        private (bool isSinked, IEnumerable<(int row, int column)> battleshipSquares) CheckIfIsSinked(int row, int column)
        {
            if (_computerGrid[row, column] != GameSquare.IsHit)
                return (false, null);

            (var hHits, var hHitSquares) = GetHorizontalHitSquares(row, column);
            if (hHits.Count() > 1 && hHitSquares.All(i => i == GameSquare.IsHit))
                return (true, hHits);

            (var vHits, var vHitSquares) = GetVerticalHitSquares(row, column);
            if (vHits.Count() > 1 && vHitSquares.All(i => i == GameSquare.IsHit))
                return (true, vHits);

            return (false, null);
        }

        private (IEnumerable<(int row, int column)> hits, IEnumerable<GameSquare> gameSquares) GetHorizontalHitSquares(int row, int column)
        {
            var hits = new List<(int row, int column)>() { (row, column) };
            var hitSquares = new List<GameSquare>() { _computerGrid[row, column] };
            var r = row - 1;
            while (r > 0 && (_computerGrid[r, column] == GameSquare.IsHit || _computerGrid[r, column] == GameSquare.IsShip))
            {
                hits.Add((r, column));
                hitSquares.Add(_computerGrid[r, column]);
                r--;
            }
            r = row + 1;
            while (r < _boardSize && (_computerGrid[r, column] == GameSquare.IsHit || _computerGrid[r, column] == GameSquare.IsShip))
            {
                hits.Add((r, column));
                hitSquares.Add(_computerGrid[r, column]);
                r++;
            }
            return (hits, hitSquares);
        }

        private (IEnumerable<(int row, int column)> hits, IEnumerable<GameSquare> gameSquares) GetVerticalHitSquares(int row, int column)
        {
            var hits = new List<(int row, int column)>() { (row, column) };
            var hitSquares = new List<GameSquare>() { _computerGrid[row, column] };
            var c = column - 1;
            while (c > 0 && (_computerGrid[row, c] == GameSquare.IsHit || _computerGrid[row, c] == GameSquare.IsShip))
            {
                hits.Add((row, c));
                hitSquares.Add(_computerGrid[row, c]);
                c--;
            }
            c = column + 1;
            while (c < _boardSize && (_computerGrid[row, c] == GameSquare.IsHit || _computerGrid[row, c] == GameSquare.IsShip))
            {
                hits.Add((row, c));
                hitSquares.Add(_computerGrid[row, c]);
                c++;
            }
            return (hits, hitSquares);
        }


        private void AddBattleShip(int size)
        {
            if(size < 2 || size > _boardSize)
            {
                throw new BoardException("Wrong Size");
            }
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
                _ = direction == Direction.Horizontal ? row++ : column++;
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
                    for(var c = -1; c <=1; c++)
                    {
                        var tRow = _fixGridIndex(row + r);
                        var tCol = _fixGridIndex(column + c);
                       
                        if (_computerGrid[tRow, tCol] == GameSquare.IsShip)
                            return false;
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
