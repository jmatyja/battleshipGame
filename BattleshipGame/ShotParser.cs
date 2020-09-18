
namespace BattleshipGame
{
    public class ShotParser
    {
        public (int column, int row) ParseShot(string shot)
        {
            var column = char.ToUpper(shot[0]) - 64;
            var row = int.Parse(shot.Substring(1));
            return (column, row);
        }
    }
}
