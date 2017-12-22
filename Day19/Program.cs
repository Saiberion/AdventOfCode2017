using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelpers;

namespace Day19
{
    class Program
    {
        static int FindMazeStart(List<string> maze)
        {
            int x;
            for (x = 0; x < maze[0].Length; x++)
            {
                if (maze[0][x] == '|')
                {
                    break;
                }
            }
            return x;
        }

        static void Main(string[] args)
        {
            List<string> maze = new List<string>();
            foreach (string line in InputLoader.LoadByLines("input.txt"))
            {
                maze.Add(line);
            }

            // Maze walker
            int x = FindMazeStart(maze);
            int y = 0;
            EDirections dir = EDirections.eDown;
            int steps = 0;
            StringBuilder sb = new StringBuilder();

            while(true)
            {
                switch(dir)
                {
                    case EDirections.eUp:
                        y--;
                        break;
                    case EDirections.eDown:
                        y++;
                        break;
                    case EDirections.eLeft:
                        x--;
                        break;
                    case EDirections.eRight:
                        x++;
                        break;
                }
                steps++;

                if (maze[y][x] == '+') // direction change
                {
                    switch (dir)
                    {
                        case EDirections.eUp:
                            if ((maze[y][x - 1] == '-') || char.IsLetter(maze[y][x - 1]))
                            {
                                dir = EDirections.eLeft;
                            }
                            else if ((maze[y][x + 1] == '-') || char.IsLetter(maze[y][x + 1]))
                            {
                                dir = EDirections.eRight;
                            }
                            break;
                        case EDirections.eDown:
                            if ((maze[y][x - 1] == '-') || char.IsLetter(maze[y][x - 1]))
                            {
                                dir = EDirections.eLeft;
                            }
                            else if ((maze[y][x + 1] == '-') || char.IsLetter(maze[y][x + 1]))
                            {
                                dir = EDirections.eRight;
                            }
                            break;
                        case EDirections.eLeft:
                            if ((maze[y - 1][x] == '|') || char.IsLetter(maze[y - 1][x]))
                            {
                                dir = EDirections.eUp;
                            }
                            else if ((maze[y + 1][x] == '|') || char.IsLetter(maze[y + 1][x]))
                            {
                                dir = EDirections.eDown;
                            }
                            break;
                        case EDirections.eRight:
                            if ((maze[y - 1][x] == '|') || char.IsLetter(maze[y - 1][x]))
                            {
                                dir = EDirections.eUp;
                            }
                            else if ((maze[y + 1][x] == '|') || char.IsLetter(maze[y + 1][x]))
                            {
                                dir = EDirections.eDown;
                            }
                            break;
                    }
                }
                else if(maze[y][x] == ' ') // Reached end of maze
                {
                    break;
                }
                else
                {
                    if (char.IsLetter(maze[y][x]))
                    {
                        sb.Append(maze[y][x]);
                    }
                }
            }
            Console.WriteLine(string.Format("Passed letters: {0}", sb.ToString()));
            Console.WriteLine(string.Format("Steps taken: {0}", steps));
            Console.ReadLine();
        }
    }
}
