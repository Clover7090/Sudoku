using CloverGame.Utils;

namespace CloverGame.Cube.Sudoku
{
    public class DataPoolManager : Singleton<DataPoolManager>
    {
        private const int ROWS = 9;

        private string[] elements = { "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        public string GetRow(int index)
        {
            if (index >= 0 && index <= ROWS)
            {
                return elements[index];
            }

            return "nil";
        }

        public string GetCol(int index)
        {
            if (index >= 0 && index <= ROWS)
            {
                return elements[index];
            }

            return "nil";
        }

        public int GetRows()
        {
            return ROWS;
        }
    }
}
