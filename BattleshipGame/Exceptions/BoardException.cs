using System;
using System.Collections.Generic;
using System.Text;

namespace BattleshipGame.Exceptions
{
    public class BoardException: Exception
    {
        public BoardException(string message) : base(message) { }
    }
}
