using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Day8
{
	class Instruction
	{
		public string Register { get; internal set; }
		public string Operation { get; internal set; }
		public int OperationValue { get; internal set; }
		public string ConditionRegister { get; internal set; }
		public string Condition { get; internal set; }
		public int ConditionValue { get; internal set; }

		public Instruction(string[] s) {
			this.Register = s[0];
			this.Operation = s[1];
			this.OperationValue = int.Parse(s[2]);
			// s[3] ist immer "if"
			this.ConditionRegister = s[4];
			this.Condition = s[5];
			this.ConditionValue = int.Parse(s[6]);
		}

		private bool isConditionValid(Dictionary<string, int> registers) {
			int condRegVal = registers[this.ConditionRegister];
			bool ret;

			switch (this.Condition) {
				case ">":
					ret = condRegVal > this.ConditionValue;
					break;
				case "<":
					ret = condRegVal < this.ConditionValue;
					break;
				case "==":
					ret = condRegVal == this.ConditionValue;
					break;
				case ">=":
					ret = condRegVal >= this.ConditionValue;
					break;
				case "<=":
					ret = condRegVal <= this.ConditionValue;
					break;
				case "!=":
					ret = condRegVal != this.ConditionValue;
					break;
				default:
					System.Diagnostics.Debug.WriteLine(string.Format("Missing condition '{0}'", this.Condition));
					ret = false;
					break;
			}
			return ret;
		}

		private void executeOperation(Dictionary<string, int> registers) {
			switch (this.Operation) {
				case "inc":
					registers[this.Register] += this.OperationValue;
					break;
				case "dec":
					registers[this.Register] -= this.OperationValue;
					break;
				default:
					System.Diagnostics.Debug.WriteLine(string.Format("Missing operation '{0}'", this.Operation));
					break;
			}
		}

		public void execute(Dictionary<string, int> registers) {
			if (isConditionValid(registers)) {
				executeOperation(registers);
			}
		}
	}

	class Program
	{
		static int executeInstructions(Dictionary<string, int> registers, List<Instruction> instructionList) {
			int ret = int.MinValue;
			int val;
			foreach (Instruction instr in instructionList) {
				instr.execute(registers);
				val = getLargestValue(registers);
				if (val > ret) {
					ret = val;
				}
			}
			return ret;
		}

		static int getLargestValue(Dictionary<string, int> registers) {
			int ret = int.MinValue;

			foreach (int val in registers.Values) {
				if (val > ret) {
					ret = val;
				}
			}
			return ret;
		}

		static void Main(string[] args) {
			StreamReader file = new StreamReader("input.txt");
			string line;
			Dictionary<string, int> registers = new Dictionary<string, int>();
			List<Instruction> instructionList = new List<Instruction>();
			int largestEver;

			while ((line = file.ReadLine()) != null) {
				string[] s = line.Split(' ');
				if (!registers.ContainsKey(s[0])) {
					registers.Add(s[0], 0);
				}
				instructionList.Add(new Instruction(s));
			}

            file.Close();

			largestEver = executeInstructions(registers, instructionList);

			Console.WriteLine(string.Format("Largest Register Value: {0}", getLargestValue(registers)));
			Console.WriteLine(string.Format("Largest Register Value Ever: {0}", largestEver));
			Console.ReadLine();
		}
	}
}
