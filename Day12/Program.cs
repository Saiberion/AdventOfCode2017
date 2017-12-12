using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace Day12
{
	class Program
	{
		static int countIDs(Dictionary<int, List<int>> pipes, List<int> groupIDs, int searchID) {
			int ret = 0;
			if (!groupIDs.Contains(searchID)) {
				ret++;
				groupIDs.Add(searchID);
				foreach (int i in pipes[searchID]) {
					ret += countIDs(pipes, groupIDs, i);
				}
			}
			return ret;
		}

		static void Main(string[] args) {
			StreamReader file = new StreamReader("input.txt");
			string line;

			Dictionary<int, List<int>> progPipes = new Dictionary<int, List<int>>();
			List<int> groups = new List<int>();

			while ((line = file.ReadLine()) != null) {
				List<int> canTalkTo = new List<int>();
				string[] s = line.Replace(" ", "").Split(new string[] { "<->", "," }, StringSplitOptions.RemoveEmptyEntries);
				canTalkTo = new List<int>();
				for (int i = 1; i < s.Length; i++) {
					canTalkTo.Add(int.Parse(s[i]));
				}
				progPipes.Add(int.Parse(s[0]), canTalkTo);
			}

			file.Close();

			while (progPipes.Count > 0) {
				List<int> groupIDs = new List<int>();
				groups.Add(countIDs(progPipes, groupIDs, progPipes.First().Key));
				foreach (int i in groupIDs) {
					progPipes.Remove(i);
				}
			}

			Console.WriteLine(string.Format("Progs in 0: {0}", groups[0]));
			Console.WriteLine(string.Format("Number of Groups: {0}", groups.Count));
			Console.ReadLine();
		}
	}
}
