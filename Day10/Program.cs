using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KnotHashing;

namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader file = new StreamReader("input.txt");
            string stream;

            stream = file.ReadToEnd();
            file.Close();

            KnotHash knotHash = new KnotHash();

            Console.WriteLine(string.Format("Result Part1: {0}", knotHash.GetHashChecksum(stream)));
            Console.WriteLine(string.Format("Result Part2: {0}", knotHash.GetHashString(stream)));
            Console.ReadLine();
        }
    }
}
