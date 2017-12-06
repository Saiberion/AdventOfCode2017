using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Day6
{
	class Program
	{
		static int calculateSignature(List<int> l) {
			StringBuilder sb = new StringBuilder();

			sb.Append(l.Count);

			for (int i = 0; i < l.Count; i++) {
				sb.Append(string.Format(";{0}", l[i]));
			}

			return sb.ToString().GetHashCode();
		}

		static int findMaxBlocks(List<int> l) {
			int maxblocks = -1;
			int index = 0;

			for (int i = 0; i < l.Count; i++) {
				if (l[i] > maxblocks) {
					maxblocks = l[i];
					index = i;
				}
			}

			return index;
		}

		static void Main(string[] args) {
			StreamReader file = new StreamReader("input.txt");
			string line;
			List<int> memoryBanks = new List<int>();
			List<int> uniqueSignatures = new List<int>();
			int steps;
			int loopsize = 0;

			while ((line = file.ReadLine()) != null) {
				string[] vals = line.Split('\t');
				foreach (string s in vals) {
					memoryBanks.Add(int.Parse(s));
				}
			}
			uniqueSignatures.Add(calculateSignature(memoryBanks));

			steps = 0;
			while (true) {
				int sig;
				int start, stop;
				start = stop = findMaxBlocks(memoryBanks);
				int tmp = memoryBanks[start];
				memoryBanks[start++] = 0;
				for (int i = 0; i < tmp; i++) {
					if (start > (memoryBanks.Count - 1)) {
						start = 0;
					}
					memoryBanks[start++]++;
				}
				steps++;
				sig = calculateSignature(memoryBanks);
				if (uniqueSignatures.Contains(sig)) {
					loopsize = steps - uniqueSignatures.IndexOf(sig);
					break;
				} else {
					uniqueSignatures.Add(calculateSignature(memoryBanks));
				}
			}

			Console.WriteLine("Steps Part 1: {0}", steps);
			Console.WriteLine("Loop size Part 2: {0}", loopsize);

			Console.ReadLine();
		}
	}
}
