﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Day7
{
	class TowerElement
	{
		public string name;
		public int weight;
		public List<TowerElement> subs = new List<TowerElement>();
		public int totalWeight;
	}

	class Program
	{
		static bool addToTower(List<TowerElement> tower, TowerElement t) {
			if (tower.Count == 0) {
				tower.Add(t);
				return true;
			} else {
				foreach (TowerElement te in tower) {
					if (te.subs.Count > 0) {
						for (int i = 0; i < te.subs.Count; i++) {
							if (te.subs[i].name.Equals(t.name)) {
								te.subs[i] = t;
								return true;
							} else {
								if (te.subs[i].subs.Count > 0) {
									if (addToTower(te.subs[i].subs, t)) {
										return true;
									}
								}
							}
						}
						if (addToTower(te.subs, t)) {
							return true;
						}
					} else {
						if (te.name.Equals(t.name)) {
							tower.Remove(te);
							tower.Add(t);
							return true;
						}
					}
				}
			}
			return false;
		}

		static bool addFromTower(List<TowerElement> tower, TowerElement t) {
			if (tower.Count == 0) {
				return false;
			} else {
				for (int i = 0; i < t.subs.Count; i++) {
					foreach (TowerElement te in tower) {
						if (te.name.Equals(t.subs[i].name)) {
							t.subs[i] = te;
							tower.Remove(te);
							break;
						}
					}
				}
			}
			return false;
		}

		static void towerWeight(TowerElement t) {
			if (t.subs.Count > 0) {
				t.totalWeight = t.weight;
				foreach(TowerElement te in t.subs) {
					towerWeight(te);
				}
				foreach (TowerElement te in t.subs) {
					t.totalWeight += te.totalWeight;
				}
			} else {
				t.totalWeight = t.weight;
			}
		}

		static void findUnbalancedElement(TowerElement tower) {
			int compare = 0;
			for (int i = 0; i < tower.subs.Count; i++) {
				if (i == 0) {
					compare = tower.subs[i].totalWeight;
				} else {
					if (compare != tower.subs[i].totalWeight) {

					}
				}
			}
		}

		static void Main(string[] args) {
			StreamReader file = new StreamReader("input.txt");
			string line;
			List<TowerElement> tower = new List<TowerElement>();

			while ((line = file.ReadLine()) != null) {
				string[] s = line.Replace(" ", "").Split(new string[] { "(", ")", "->" }, StringSplitOptions.RemoveEmptyEntries);
				TowerElement t = new TowerElement();
				t.name = s[0];
				t.weight = int.Parse(s[1]);
				if (s.Length > 2) {
					string[] csv = s[2].Split(',');
					foreach (string n in csv) {
						TowerElement te = new TowerElement();
						te.name = n;
						t.subs.Add(te);
					}
					addFromTower(tower, t);
				}

				if (!addToTower(tower, t)) {
					// Turm hat schon eine Basis, kann nirgends eingehängt werden
					tower.Add(t);
				}
			}

			towerWeight(tower[0]);

			Console.WriteLine("Towerbase Part 1: {0}", tower[0].name);
			Console.ReadLine();
		}
	}
}
