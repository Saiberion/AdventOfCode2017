using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day10
{
    class Program
    {
        static int currentPosition = 0;
        static int skipSize = 0;
        const int listLength = 256;

        static void knotHashingRound(List<byte> knotHash, List<byte> inputLengths)
        {
            foreach (int i in inputLengths)
            {
                List<byte> segment;
                try
                {
                    segment = knotHash.GetRange(currentPosition, i);
                    segment.Reverse();
                    knotHash.RemoveRange(currentPosition, i);
                    knotHash.InsertRange(currentPosition, segment);
                }
                catch (ArgumentException e)
                {
                    segment = knotHash.GetRange(currentPosition, listLength - currentPosition);
                    segment.AddRange(knotHash.GetRange(0, i - (listLength - currentPosition)));
                    segment.Reverse();
                    knotHash.RemoveRange(currentPosition, listLength - currentPosition);
                    knotHash.AddRange(segment.GetRange(0, listLength - currentPosition));
                    knotHash.RemoveRange(0, i - (knotHash.Count - currentPosition));
                    knotHash.InsertRange(0, segment.GetRange(listLength - currentPosition, i - (listLength - currentPosition)));
                }

                currentPosition += i + skipSize;
                while (currentPosition >= listLength)
                {
                    currentPosition -= listLength;
                }
                skipSize++;
            }
        }

        static int part1(string stream)
        {
            List<byte> inputLengths = new List<byte>();
            List<byte> knotHash = new List<byte>();

            currentPosition = 0;
            skipSize = 0;

            foreach (string s in stream.Split(','))
            {
                inputLengths.Add(byte.Parse(s));
            }

            for (int i = 0; i < listLength; i++)
            {
                knotHash.Add((byte)i);
            }

            knotHashingRound(knotHash, inputLengths);

            return knotHash[0] * knotHash[1];
        }

        static string part2(string stream)
        {
            List<byte> inputLengths = new List<byte>();
            List<byte> knotHash = new List<byte>();
            List<byte> denseHash = new List<byte>();

            currentPosition = 0;
            skipSize = 0;

            inputLengths.AddRange(Encoding.ASCII.GetBytes(stream));

            foreach (string s in "17,31,73,47,23".Split(','))
            {
                inputLengths.Add(byte.Parse(s));
            }

            for (int i = 0; i < listLength; i++)
            {
                knotHash.Add((byte)i);
            }

            for (int i = 0; i < 64; i++)
            {
                knotHashingRound(knotHash, inputLengths);
            }

            for (int i = 0; i < 16; i++)
            {
                byte xorblock = 0;
                for (int j = 0; j < 16; j++)
                {
                    xorblock ^= knotHash[i * 16 + j];
                }
                denseHash.Add(xorblock);
            }

            StringBuilder sb = new StringBuilder();
            foreach (byte b in denseHash)
            {
                sb.Append(string.Format("{0:x2}", b));
            }

            return sb.ToString();
        }

        static void Main(string[] args)
        {
            StreamReader file = new StreamReader("input.txt");
            string stream;

            stream = file.ReadToEnd();
            file.Close();

            Console.WriteLine(string.Format("Result Part1: {0}", part1(stream)));
            Console.WriteLine(string.Format("Result Part2: {0}", part2(stream)));
            Console.ReadLine();
        }
    }
}
