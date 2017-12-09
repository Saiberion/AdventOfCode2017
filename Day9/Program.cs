using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day9
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader file = new StreamReader("input.txt");
            string stream;
            int currentGroupScore = 0;
            int totalScore = 0;
            bool openGarbage = false;
            int garbageCharachters = 0;

            stream = file.ReadToEnd();
            file.Close();

            for (int i = 0; i < stream.Length; i++)
            {
                switch(stream[i])
                {
                    case '{':
                        if (!openGarbage)
                        {
                            currentGroupScore++;
                        }
                        else
                        {
                            garbageCharachters++;
                        }
                        break;
                    case '}':
                        if (!openGarbage)
                        {
                            totalScore += currentGroupScore;
                            currentGroupScore--;
                        }
                        else
                        {
                            garbageCharachters++;
                        }
                        break;
                    case '<':
                        if (!openGarbage)
                        {
                            openGarbage = true;
                        }
                        else
                        {
                            garbageCharachters++;
                        }
                        break;
                    case '>':
                        openGarbage = false;
                        break;
                    case '!':
                        if (openGarbage)
                        {
                            i++;
                        }
                        break;
                    default:
                        if (openGarbage)
                        {
                            garbageCharachters++;
                        }
                        break;
                }

               
            }

            Console.WriteLine(string.Format("Total score: {0}", totalScore));
            Console.WriteLine(string.Format("Garbage characters: {0}", garbageCharachters));
            Console.ReadLine();
        }
    }
}
