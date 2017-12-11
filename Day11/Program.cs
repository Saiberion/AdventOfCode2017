using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Day11
{
	class HexGridPosition
	{
		public int X { get; set; }
		public int Y { get; set; }
		public int Z { get; set; }

		public HexGridPosition() {
			this.X = 0;
			this.Y = 0;
			this.Z = 0;
		}

		public void move(string dir) {
			switch (dir) {
				case "n":
					this.X--;
					this.Y++;
					break;
				case "ne":
					this.Y++;
					this.Z--;
					break;
				case "nw":
					this.X--;
					this.Z++;
					break;
				case "s":
					this.X++;
					this.Y--;
					break;
				case "sw":
					this.Y--;
					this.Z++;
					break;
				case "se":
					this.X++;
					this.Z--;
					break;
			}
		}

		public int distance(HexGridPosition pos) {
			int diffx, diffy, diffz;
			int distance;

			diffx = Math.Abs(pos.X - this.X);
			diffy = Math.Abs(pos.Y - this.Y);
			diffz = Math.Abs(pos.Z - this.Z);

			distance = Math.Max(Math.Max(diffx, diffy), diffz);

			return distance;
		}
	}

	class Program
	{
		static void Main(string[] args) {
			StreamReader file = new StreamReader("input.txt");
			string stream = file.ReadLine();

			file.Close();

			HexGridPosition start = new HexGridPosition();
			HexGridPosition goal = new HexGridPosition();
			string[] directions = stream.Split(',');
			int maxDistance = int.MinValue;
			int distance;

			foreach (string dir in directions) {
				goal.move(dir);
				distance = goal.distance(start);
				if (distance > maxDistance) {
					maxDistance = distance;
				}
			}

			Console.WriteLine(string.Format("End Distance: {0}", goal.distance(start)));
			Console.WriteLine(string.Format("Max Distance: {0}", maxDistance));
			Console.ReadLine();
		}
	}
}
