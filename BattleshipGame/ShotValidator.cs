
namespace BattleshipGame
{
    public class ShotValidator
    {
        private int _boardSize;
        public ShotValidator(int boardSize)
        {
            _boardSize = boardSize;
        }
        public bool IsValid(string shot)
        {
            return _validateColumn(shot[0]) && _validateRow(shot.Substring(1));
        }

        private bool _validateColumn(char column)
        {
            var columnNumber = char.ToUpper(column) - 64;
            return columnNumber > 0 && columnNumber <= _boardSize;
        }

        private bool _validateRow(string row)
        {
            int rowNumber;
            var isValid = int.TryParse(row, out rowNumber);
            return isValid && rowNumber > 0 && rowNumber <= _boardSize;
        }
    }
}
