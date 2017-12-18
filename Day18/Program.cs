using System;
using System.Collections.Concurrent;
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

    class DuetThread
    {
        public List<Instruction> Program { get; set; }
        public int ID { get; set; }
        public BlockingCollection<Int64> SendQueue { get; set; }
        public BlockingCollection<Int64> ReceiveQueue { get; set; }

        public void thread()
        {
            Int64 programCounter = 0;
            Dictionary<string, Int64> registerMap = new Dictionary<string, Int64>();
            BlockingCollection<Int64> sndQ = this.SendQueue;
            BlockingCollection<Int64> rcvQ = this.ReceiveQueue;
            int sendCounter = 0;

            registerMap.Add("p", this.ID);

            do
            {
                Int64 parameter;
                Instruction instr = Program[(int)programCounter];

                if (!registerMap.ContainsKey(instr.Register))
                {
                    if (!Int64.TryParse(instr.Register, out parameter))
                    {
                        registerMap.Add(instr.Register, 0);
                    }
                    else
                    {
                        registerMap.Add(instr.Register, parameter);
                    }
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
                        sndQ.Add(registerMap[instr.Register]);
                        sendCounter++;
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
                        if (rcvQ.TryTake(out parameter, 200))
                        {
                            registerMap[instr.Register] = parameter;
                            programCounter++;
                        }
                        else
                        {
                            programCounter = -1;
                        }
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
            } while ((programCounter >= 0) && (programCounter < Program.Count));
            Console.WriteLine(string.Format("Prog {0} sendCounter at dead lock: {1}", this.ID, sendCounter));
        }
    }

    class Program
    {
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

            DuetThread dt1 = new DuetThread();
            DuetThread dt2 = new DuetThread();

            System.Threading.Thread p1 = new System.Threading.Thread(new System.Threading.ThreadStart(dt1.thread));
            System.Threading.Thread p2 = new System.Threading.Thread(new System.Threading.ThreadStart(dt2.thread));

            dt1.Program = program;
            dt2.Program = program;

            dt1.ID = 0;
            dt2.ID = 1;

            dt1.SendQueue = new BlockingCollection<Int64>();
            dt2.SendQueue = new BlockingCollection<Int64>();

            dt1.ReceiveQueue = dt2.SendQueue;
            dt2.ReceiveQueue = dt1.SendQueue;

            p1.Start();
            p2.Start();

            p1.Join();
            p2.Join();

            Console.WriteLine(string.Format("Program ended"));
            Console.ReadLine();
        }
    }
}
