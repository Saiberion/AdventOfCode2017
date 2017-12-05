﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Day5
{
	class Program
	{
		static int listWalkerPart1(List<int> jumpList) {
			int steps = 0;
			int index = 0;
			List<int> jList = new List<int>(jumpList);

			while (true) {
				steps++;
				index += jList[index]++;
				if (index >= jList.Count) {
					break;
				}
			}

			return steps;
		}

		static int listWalkerPart2(List<int> jumpList) {
			int steps = 0;
			int index = 0;
			List<int> jList = new List<int>(jumpList);

			while (true) {
				int listVal = jList[index];
				int prevIndex = index;
				steps++;
				index += jList[index];
				if (jList[prevIndex] >= 3) {
					jList[prevIndex]--;
				} else {
					jList[prevIndex]++;
				}
				if (index >= jList.Count) {
					break;
				}
			}

			return steps;
		}

		static void Main(string[] args) {
			StreamReader file = new StreamReader("input.txt");
			string line;
			List<int> jumpList = new List<int>();

			while ((line = file.ReadLine()) != null) {
				jumpList.Add(int.Parse(line));
			}

			Console.WriteLine("Jumps Part 1: {0}", listWalkerPart1(jumpList));
			Console.WriteLine("Jumps Part 2: {0}", listWalkerPart2(jumpList));

			Console.ReadLine();
		}
	}
}