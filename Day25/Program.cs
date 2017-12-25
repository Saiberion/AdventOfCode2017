using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day25
{
    enum EStates
    {
        eA,
        eB,
        eC,
        eD,
        eE,
        eF
    }

    class Program
    {
        static void Main(string[] args)
        {
            int position = 0;
            EStates state = EStates.eA;
            Dictionary<int, int> tape = new Dictionary<int, int>();
            int checksum = 0;

            tape.Add(0, 0);

            for (int i = 0; i < 12523873; i++)
            {
                switch (state)
                {
                    case EStates.eA:
                        if (tape[position] == 0)
                        {
                            tape[position] = 1;
                            position++;
                            state = EStates.eB;
                        }
                        else
                        {
                            tape[position] = 1;
                            position--;
                            state = EStates.eE;
                        }
                        break;
                    case EStates.eB:
                        if (tape[position] == 0)
                        {
                            tape[position] = 1;
                            position++;
                            state = EStates.eC;
                        }
                        else
                        {
                            tape[position] = 1;
                            position++;
                            state = EStates.eF;
                        }
                        break;
                    case EStates.eC:
                        if (tape[position] == 0)
                        {
                            tape[position] = 1;
                            position--;
                            state = EStates.eD;
                        }
                        else
                        {
                            tape[position] = 0;
                            position++;
                            state = EStates.eB;
                        }
                        break;
                    case EStates.eD:
                        if (tape[position] == 0)
                        {
                            tape[position] = 1;
                            position++;
                            state = EStates.eE;
                        }
                        else
                        {
                            tape[position] = 0;
                            position--;
                            state = EStates.eC;
                        }
                        break;
                    case EStates.eE:
                        if (tape[position] == 0)
                        {
                            tape[position] = 1;
                            position--;
                            state = EStates.eA;
                        }
                        else
                        {
                            tape[position] = 0;
                            position++;
                            state = EStates.eD;
                        }
                        break;
                    case EStates.eF:
                        if (tape[position] == 0)
                        {
                            tape[position] = 1;
                            position++;
                            state = EStates.eA;
                        }
                        else
                        {
                            tape[position] = 1;
                            position++;
                            state = EStates.eC;
                        }
                        break;
                }
                if (!tape.ContainsKey(position))
                {
                    tape.Add(position, 0);
                }
            }

            foreach(int v in tape.Values)
            {
                checksum += v;
            }

            Console.WriteLine(string.Format("Diagnostics checksum: {0}", checksum));
            Console.ReadLine();
        }
    }
}
