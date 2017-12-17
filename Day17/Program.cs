using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day17
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> spinLock = new List<int>();
            int currentPosition = 0;
            int valueAt1 = 0;

            // Part 1
            spinLock.Add(0);
            for (int i = 1; i <= 2017; i++)
            {
                currentPosition = (currentPosition + 359) % i + 1;
                spinLock.Insert(currentPosition, i);
            }

            Console.WriteLine(string.Format("Next Value: {0}", spinLock[currentPosition + 1]));

            // Part 2
            currentPosition = 0;
            for (int i = 1; i <= 50000000; i++)
            {
                currentPosition = (currentPosition + 359) % i + 1;
                if (currentPosition == 1)
                {
                    valueAt1 = i;
                }
            }

            Console.WriteLine(string.Format("Value after 0: {0}", valueAt1));
            Console.ReadLine();
        }
    }
}
