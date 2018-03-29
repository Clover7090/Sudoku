using System;
using CloverGame.Cube.Sudoku;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku
{
    class Program
    {
        static void Main(string[] args)
        {
            int succCount = 0;
            for(int i= 0; i< 10;i++)
            {
                CreatePoolManager.Instance.Init();
                succCount +=CreatePoolManager.Instance.Generate();
            }
            Console.WriteLine("succCount:" + succCount);
            Console.ReadLine();           
        }        
    }
}
