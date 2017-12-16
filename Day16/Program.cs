using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    class DanceMove
    {
        public char Move { get; set; }
        public List<object>Operands { get; set; }

        public DanceMove(string move)
        {
            string[] tmp;
            this.Move = move[0];
            this.Operands = new List<object>();
            switch(this.Move)
            {
                case 's':
                    this.Operands.Add(int.Parse(move.Remove(0, 1)));
                    break;
                case 'x':
                    tmp = move.Remove(0, 1).Split('/');
                    this.Operands.Add(int.Parse(tmp[0]));
                    this.Operands.Add(int.Parse(tmp[1]));
                    break;
                case 'p':
                    tmp = move.Remove(0, 1).Split('/');
                    this.Operands.Add(tmp[0]);
                    this.Operands.Add(tmp[1]);
                    break;
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader file = new StreamReader("input.txt");
            string dance;

            dance = file.ReadLine();

            file.Close();

            List<char> dancers = new List<char>();
            for(int i = 0; i < 16; i++)
            {
                dancers.Add(Convert.ToChar('a' + i));
            }
            string[] danceMoves = dance.Split(',');
            List<DanceMove> listDanceMoves = new List<DanceMove>();
            for(int i = 0; i < danceMoves.Length; i++)
            {
                listDanceMoves.Add(new DanceMove(danceMoves[i]));
            }

            List<string> dancePattern = new List<string>();
            StringBuilder sb = new StringBuilder();
            int indA, indB;
            char tmp;
            int repeatsAfter = 0;

            for (int i = 0; i < dancers.Count; i++)
            {
                sb.Append(dancers[i].ToString());
            }
            dancePattern.Add(sb.ToString());

            do
            {
                for (int i = 0; i < listDanceMoves.Count; i++)
                {
                    switch (listDanceMoves[i].Move)
                    {
                        case 's':
                            int rotate = (int)listDanceMoves[i].Operands[0];
                            dancers = dancers.Skip(dancers.Count - rotate)
                            .Take(rotate)
                            .Concat(dancers.Take(dancers.Count - rotate))
                            .ToList();
                            break;
                        case 'x':
                            indA = (int)listDanceMoves[i].Operands[0];
                            indB = (int)listDanceMoves[i].Operands[1];
                            char a = dancers[indA];
                            char b = dancers[indB];
                            dancers[indA] = b;
                            dancers[indB] = a;
                            break;
                        case 'p':
                            indA = dancers.IndexOf(((string)listDanceMoves[i].Operands[0])[0]);
                            indB = dancers.IndexOf(((string)listDanceMoves[i].Operands[1])[0]);
                            tmp = dancers[indA];
                            dancers[indA] = dancers[indB];
                            dancers[indB] = tmp;
                            break;
                    }
                }

                sb.Clear();
                for (int i = 0; i < dancers.Count; i++)
                {
                    sb.Append(dancers[i].ToString());
                }
                dancePattern.Add(sb.ToString());
            } while (!dancePattern[0].Equals(dancePattern[++repeatsAfter]));
            dancePattern.RemoveAt(repeatsAfter - 1);

            Console.WriteLine(string.Format("Dancers Part1: {0}", dancePattern[1]));
            Console.WriteLine(string.Format("Dancers Part2: {0}", dancePattern[1000000 % repeatsAfter]));
            Console.ReadLine();
        }
    }
}
