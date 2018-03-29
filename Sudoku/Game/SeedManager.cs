using CloverGame.Utils;
using System;

namespace CloverGame.Cube.Sudoku
{
    public class SeedManager : Singleton<SeedManager>
    {
        private Random random;

        public SeedManager()
        {
            random = new Random();
        }

        public int Seed(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }

        public int Seed(int[] arr)
        {
            return Seed(0, arr.Length);
        }
    }
}
