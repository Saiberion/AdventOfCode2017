using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day14
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
                catch (ArgumentException)
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

        static string knotHash(string stream)
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

        static void fillRegion(int regionNumber, int x, int y, List<int[]> grid)
        {
            grid[y][x] = regionNumber;

            if ((x > 0) && (grid[y][x - 1] == -1))
            {
                fillRegion(regionNumber, x - 1, y, grid);
            }
            if ((x < 127) && (grid[y][x + 1] == -1))
            {
                fillRegion(regionNumber, x + 1, y, grid);
            }
            if ((y > 0) && (grid[y - 1][x] == -1))
            {
                fillRegion(regionNumber, x, y - 1, grid);
            }
            if ((y < 127) && (grid[y + 1][x] == -1))
            {
                fillRegion(regionNumber, x, y + 1, grid);
            }
        }

        static void Main(string[] args)
        {
            StreamReader file = new StreamReader("input.txt");
            string baseKey;

            baseKey = file.ReadLine();

            file.Close();

            List<int[]> diskGrid = new List<int[]>();
            int usedGridCells = 0;
            int regionCount = 0;

            for (int i = 0; i < 128; i++)
            {
                int[] arr = new int[128];
                int baseIndex = 0;
                string hash = knotHash(string.Format("{0}-{1}", baseKey, i));
                foreach (char c in hash)
                {
                    byte b = Convert.ToByte(c.ToString(), 16);
                    for (int k = 3; k >= 0; k--)
                    {
                        if ((b & (1 << k)) > 0)
                        {
                            usedGridCells++;
                            arr[baseIndex++] = -1;
                        } else
                        {
                            arr[baseIndex++] = 0;
                        }
                    }
                }
                diskGrid.Add(arr);
            }

            for (int y = 0; y < 128; y++)
            {
                for(int x = 0; x < 128; x++)
                {
                    if (diskGrid[y][x] == -1)
                    {
                        fillRegion(++regionCount, x, y, diskGrid);
                    }
                }
            }

            Console.WriteLine(string.Format("Used Cells: {0}", usedGridCells));
            Console.WriteLine(string.Format("Regions: {0}", regionCount));
            Console.ReadLine();
        }
    }
}
