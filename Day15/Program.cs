using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day15
{
    class Generator
    {
        private UInt64 previousValue;
        private UInt64 factor;
        private const UInt64 modulo = 2147483647;

        public Generator(UInt64 initialValue, UInt64 factor)
        {
            this.previousValue = initialValue;
            this.factor = factor;
        }

        private UInt64 Generate()
        {
            UInt64 mult = this.previousValue * this.factor;
            this.previousValue = (uint)(mult % modulo);
            return this.previousValue;
        }

        public UInt64 GenerateNext(UInt64 divisableBy)
        {
            if (divisableBy == 0)
            {
                return Generate();
            }
            else
            {
                UInt64 result;
                while (((result = Generate()) % divisableBy) != 0) ;
                return result;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Generator genAExample, genBExample;
            Generator genA, genB;
            int matchesPart1 = 0;
            int matchesPart2 = 0;

            genAExample = new Generator(65, 16807);
            genBExample = new Generator(8921, 48271);

            genA = new Generator(289, 16807);
            genB = new Generator(629, 48271);

            for (int i = 0; i < 40000000; i++)
            {
                UInt64 a, b;
                //a = genAExample.GenerateNext(0);
                //b = genBExample.GenerateNext(0);
                a = genA.GenerateNext(0);
                b = genB.GenerateNext(0);

                if ((a & 0xffff) == (b & 0xffff))
                {
                    matchesPart1++;
                }
            }

            genAExample = new Generator(65, 16807);
            genBExample = new Generator(8921, 48271);

            genA = new Generator(289, 16807);
            genB = new Generator(629, 48271);

            for (int i = 0; i < 5000000; i++)
            {
                UInt64 a, b;
                //a = genAExample.GenerateNext(4);
                //b = genBExample.GenerateNext(8);
                a = genA.GenerateNext(4);
                b = genB.GenerateNext(8);

                if ((a & 0xffff) == (b & 0xffff))
                {
                    matchesPart2++;
                }
            }

            Console.WriteLine(string.Format("Matches Part1: {0}", matchesPart1));
            Console.WriteLine(string.Format("Matches Part2: {0}", matchesPart2));
            Console.ReadLine();
        }
    }
}
