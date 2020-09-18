using BattleshipGame;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BattleshipGameTests
{
    public class ShotParserTests
    {
        [Fact]
        public void ShouldReturnCorrectColumnAndRowNumber()
        {
            var shotParser = new ShotParser();
            (var column, var row) = shotParser.ParseShot("A3");
            Assert.Equal(1, column);
            Assert.Equal(3, row);
        }

        [Fact]
        public void ShouldReturnCorrectRowNumberWhenBiggerThan9()
        {
            var shotParser = new ShotParser();
            (var column, var row) = shotParser.ParseShot("A156");
            Assert.Equal(156, row);
        }

    }
}
