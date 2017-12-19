using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Day4
{
	class Program
	{
		static int IsValidPassphrasePart1(string line) {
			string[] wordList = line.Split(' ');

			for (int i = 0; i < wordList.Length; i++) {
				for (int j = i + 1; j < wordList.Length; j++) {
					if (wordList[i].CompareTo(wordList[j]) == 0) {
						return 0;
					}
				}
			}

			return 1;
		}

		static bool IsAnagram(string w1, string w2) {
			if (w1.Length == w2.Length) {
				for (int i = 0; i < w1.Length; i++) {
					for (int j = 0; j < w2.Length; j++) {
						if (w1[i] == w2[j]) {
							w1 = w1.Remove(i--, 1);
							w2 = w2.Remove(j--, 1);
							break;
						}
					}
					if (w1.Length == 0) {
						return true;
					}
				}
			}
			return false;
		}
		
		static int IsValidPassphrasePart2(string line) {
			string[] wordList = line.Split(' ');

			for (int i = 0; i < wordList.Length; i++) {
				for (int j = i + 1; j < wordList.Length; j++) {
					if (IsAnagram(wordList[i], wordList[j])) {
						return 0;
					}
				}
			}

			return 1;
		}

		static void Main(string[] args) {
			StreamReader file = new StreamReader("input.txt");
			string line;
			int validPassphrasesPart1 = 0;
			int validPassphrasesPart2 = 0;

			while ((line = file.ReadLine()) != null) {
				validPassphrasesPart1 += IsValidPassphrasePart1(line);
				validPassphrasesPart2 += IsValidPassphrasePart2(line);
			}

			file.Close();

			Console.WriteLine("Valid Passphrases Part 1: {0}", validPassphrasesPart1);
			Console.WriteLine("Valid Passphrases Part 2: {0}", validPassphrasesPart2);
			Console.ReadLine();
		}
	}
}
