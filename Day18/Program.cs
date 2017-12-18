using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day18
{
    enum EOpcodes
    {
        eOpcodeSND,
        eOpcodeSET,
        eOpcodeADD,
        eOpcodeMUL,
        eOpcodeMOD,
        eOpcodeRCV,
        eOpcodeJGZ
    }

    class Instruction
    {
        public EOpcodes Opcode { get; set; }
        public string Register { get; set; }
        public string Parameter { get; set; }

        public Instruction(string instruction)
        {
            string[] instr = instruction.Split(' ');

            switch(instr[0])
            {
                case "snd":
                    this.Opcode = EOpcodes.eOpcodeSND;
                    break;
                case "set":
                    this.Opcode = EOpcodes.eOpcodeSET;
                    break;
                case "add":
                    this.Opcode = EOpcodes.eOpcodeADD;
                    break;
                case "mul":
                    this.Opcode = EOpcodes.eOpcodeMUL;
                    break;
                case "mod":
                    this.Opcode = EOpcodes.eOpcodeMOD;
                    break;
                case "rcv":
                    this.Opcode = EOpcodes.eOpcodeRCV;
                    break;
                case "jgz":
                    this.Opcode = EOpcodes.eOpcodeJGZ;
                    break;
            }

            this.Register = instr[1];

            if (instr.Length > 2)
            {
                this.Parameter = instr[2];
            }
            else
            {
                this.Parameter = string.Empty;
            }
        }
    }

    class MyThreadObject
    {
        public List<Instruction> instructionList;
        public int localID;
        public int remoteID;
    }

    class Program
    {
        static Queue<Int64>[] messageQueues = new Queue<Int64>[2];

        static void Main(string[] args)
        {
            StreamReader file = new StreamReader("input.txt");
            string line;

            List<Instruction> program = new List<Instruction>();
            Dictionary<string, Int64> registerMap = new Dictionary<string, Int64>();
            Int64 programCounter = 0;
            Int64 lastFrequency = 0;

            while ((line = file.ReadLine()) != null)
            {
                program.Add(new Instruction(line));
            }

            do
            {
                Int64 parameter;
                Instruction instr = program[(int)programCounter];

                if (!registerMap.ContainsKey(instr.Register))
                {
                    registerMap.Add(instr.Register, 0);
                }

                if (!Int64.TryParse(instr.Parameter, out parameter))
                {
                    if (registerMap.ContainsKey(instr.Parameter))
                    {
                        parameter = registerMap[instr.Parameter];
                    }
                }

                switch (instr.Opcode)
                {
                    case EOpcodes.eOpcodeSND:
                        lastFrequency = registerMap[instr.Register];
                        programCounter++;
                        break;
                    case EOpcodes.eOpcodeSET:
                        registerMap[instr.Register] = parameter;
                        programCounter++;
                        break;
                    case EOpcodes.eOpcodeADD:
                        registerMap[instr.Register] += parameter;
                        programCounter++;
                        break;
                    case EOpcodes.eOpcodeMUL:
                        registerMap[instr.Register] *= parameter;
                        programCounter++;
                        break;
                    case EOpcodes.eOpcodeMOD:
                        registerMap[instr.Register] %= parameter;
                        programCounter++;
                        break;
                    case EOpcodes.eOpcodeRCV:
                        if (registerMap[instr.Register] != 0)
                        {
                            Console.WriteLine(string.Format("Successfully recoverd frequency {0}", lastFrequency));
                            programCounter = -1;
                        }
                        programCounter++;
                        break;
                    case EOpcodes.eOpcodeJGZ:
                        if (registerMap[instr.Register] > 0)
                        {
                            programCounter += parameter;
                        }
                        else
                        {
                            programCounter++;
                        }
                        break;
                }
            } while ((programCounter >= 0) && (programCounter < program.Count));

            // Damn snd and rcv do something completely different :-)

            messageQueues[0] = new Queue<Int64>();
            messageQueues[1] = new Queue<Int64>();

            System.Threading.Thread p1 = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(assembly));
            System.Threading.Thread p2 = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(assembly));

            MyThreadObject tparam1 = new MyThreadObject();
            MyThreadObject tparam2 = new MyThreadObject();

            tparam1.instructionList = program;
            tparam2.instructionList = program;

            tparam1.localID = 0;
            tparam2.localID = 1;

            tparam1.remoteID = 1;
            tparam2.remoteID = 0;

            p1.Start(tparam1);
            p2.Start(tparam2);

            p1.Join();
            p2.Join();

            Console.WriteLine(string.Format("Program ended"));
            Console.ReadLine();
        }

        public static void assembly(object o)
        {
            MyThreadObject param = (MyThreadObject)o;
            Int64 programCounter = 0;
            Dictionary<string, Int64> registerMap = new Dictionary<string, Int64>();

            do
            {
                Int64 parameter;
                Instruction instr = param.instructionList[(int)programCounter];

                if (!registerMap.ContainsKey(instr.Register))
                {
                    registerMap.Add(instr.Register, 0);
                }

                if (!Int64.TryParse(instr.Parameter, out parameter))
                {
                    if (registerMap.ContainsKey(instr.Parameter))
                    {
                        parameter = registerMap[instr.Parameter];
                    }
                }

                switch (instr.Opcode)
                {
                    case EOpcodes.eOpcodeSND:
                        messageQueues[param.remoteID].Enqueue(registerMap[instr.Register]);
                        programCounter++;
                        break;
                    case EOpcodes.eOpcodeSET:
                        registerMap[instr.Register] = parameter;
                        programCounter++;
                        break;
                    case EOpcodes.eOpcodeADD:
                        registerMap[instr.Register] += parameter;
                        programCounter++;
                        break;
                    case EOpcodes.eOpcodeMUL:
                        registerMap[instr.Register] *= parameter;
                        programCounter++;
                        break;
                    case EOpcodes.eOpcodeMOD:
                        registerMap[instr.Register] %= parameter;
                        programCounter++;
                        break;
                    case EOpcodes.eOpcodeRCV:
                        registerMap[instr.Register] = messageQueues[param.localID].Dequeue();
                        programCounter++;
                        break;
                    case EOpcodes.eOpcodeJGZ:
                        if (registerMap[instr.Register] > 0)
                        {
                            programCounter += parameter;
                        }
                        else
                        {
                            programCounter++;
                        }
                        break;
                }
            } while ((programCounter >= 0) && (programCounter < param.instructionList.Count));
        }
    }
}
