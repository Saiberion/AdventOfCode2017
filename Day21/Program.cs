using AoCHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;

namespace Day21
{
    class Program
    {
        /*static char[,] RotateMatrix(char[,] matrix, int n)
        {
            char[,] ret = new char[n, n];

            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    ret[i, j] = matrix[n - j - 1, i];
                }
            }

            return ret;
        }*/

        static void Main(string[] args)
        {
            List<string> startPattern = new List<string>();
            startPattern.Add(".#.");
            startPattern.Add("..#");
            startPattern.Add("###");

            List<string> outputPattern = new List<string>(startPattern);

            List<string[]> transformRules = new List<string[]>();
            foreach (string line in InputLoader.LoadByLines("input.txt"))
            {
                transformRules.Add(line.Split(new string[] { " => " }, StringSplitOptions.RemoveEmptyEntries));
            }

            if ((outputPattern.Count % 2) == 0)
            {

            }
            else
            {
                if (outputPattern.Count > 3)
                {

                }
                else
                {
                    
                }
            }

            /*char[,] matrix = new char[,] { { '1', '2', '3' }, { '4', '5', '6' }, { '7', '8', '9' } };
            matrix = RotateMatrix(matrix, 2);*/

            Console.ReadLine();
        }
    }
}
