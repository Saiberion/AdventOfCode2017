using System;
using System.Collections.Generic;
using System.Text;
using AoCHelpers;

namespace Day1
{
	class Program
	{
        static int SumUp(string input, int compareSkip)
        {
            int result = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == input[(i + compareSkip) % input.Length])
                {
                    result += input[i] - '0';
                }
            }
            return result;
        }

        static void Main(string[] args)
        {
            string input = InputLoader.LoadByLines("input.txt")[0];

            Console.WriteLine(string.Format("Sum Part 1: {0}", SumUp(input, 1)));
            Console.WriteLine(string.Format("Sum Part 2: {0}", SumUp(input, input.Length / 2)));
            Console.ReadLine();
        }
	}
}
