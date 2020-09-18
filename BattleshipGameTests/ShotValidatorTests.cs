using BattleshipGame;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BattleshipGameTests
{
    public class ShotValidatorTests
    {
        [Theory]
        [InlineData("AB156")]
        [InlineData("#23")]
        [InlineData("Bo31")]
        [InlineData("Z1")]
        public void ShouldReturnFalseWhenIncorectFormat(string format)
        {
            var shotValidator = new ShotValidator(10);
            var isValid = shotValidator.IsValid(format);
            Assert.False(isValid);
        }
    }
}
