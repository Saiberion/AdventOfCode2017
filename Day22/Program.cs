using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelpers;

namespace Day22
{
    class VirusCarrierSimple
    {
        public Dictionary<int, Dictionary<int, bool>> InfectionMap { get; set; }
        public int InfectionBursts { get; set; }

        int currentXPos, currentYPos;
        EDirections direction;

        public VirusCarrierSimple(List<string> gridMap)
        {
            InfectionMap = new Dictionary<int, Dictionary<int, bool>>();
            for (int y = 0; y < gridMap.Count; y++)
            {
                InfectionMap.Add(y, new Dictionary<int, bool>());
                for(int x = 0; x < gridMap[y].Length; x++)
                {
                    bool val = gridMap[y][x] == '#' ? true : false;
                    InfectionMap[y].Add(x, val);
                }
            }
            direction = EDirections.eUp;
            currentYPos = InfectionMap.Count / 2;
            currentXPos = InfectionMap[0].Count / 2;
            InfectionBursts = 0;
        }

        public void Move()
        {
            if (InfectionMap[currentYPos][currentXPos])
            {
                switch(direction)
                {
                    case EDirections.eUp:
                        direction = EDirections.eRight;
                        break;
                    case EDirections.eDown:
                        direction = EDirections.eLeft;
                        break;
                    case EDirections.eLeft:
                        direction = EDirections.eUp;
                        break;
                    case EDirections.eRight:
                        direction = EDirections.eDown;
                        break;
                }
            }
            else
            {
                InfectionBursts++;
                switch (direction)
                {
                    case EDirections.eUp:
                        direction = EDirections.eLeft;
                        break;
                    case EDirections.eDown:
                        direction = EDirections.eRight;
                        break;
                    case EDirections.eLeft:
                        direction = EDirections.eDown;
                        break;
                    case EDirections.eRight:
                        direction = EDirections.eUp;
                        break;
                }
            }
            InfectionMap[currentYPos][currentXPos] = !InfectionMap[currentYPos][currentXPos];

            switch (direction)
            {
                case EDirections.eUp:
                    currentYPos--;
                    break;
                case EDirections.eDown:
                    currentYPos++;
                    break;
                case EDirections.eLeft:
                    currentXPos--;
                    break;
                case EDirections.eRight:
                    currentXPos++;
                    break;
            }
            if (!InfectionMap.ContainsKey(currentYPos))
            {
                InfectionMap.Add(currentYPos, new Dictionary<int, bool>());
            }
            if (!InfectionMap[currentYPos].ContainsKey(currentXPos))
            {
                InfectionMap[currentYPos].Add(currentXPos, false);
            }
        }
    }

    enum ENodeStates
    {
        eClean,
        eWeakened,
        eInfected,
        eFlagged
    }

    class VirusCarrierEnhanced
    {
        public Dictionary<int, Dictionary<int, ENodeStates>> InfectionMap { get; set; }
        public int InfectionBursts { get; set; }

        int currentXPos, currentYPos;
        EDirections direction;

        public VirusCarrierEnhanced(List<string> gridMap)
        {
            InfectionMap = new Dictionary<int, Dictionary<int, ENodeStates>>();
            for (int y = 0; y < gridMap.Count; y++)
            {
                InfectionMap.Add(y, new Dictionary<int, ENodeStates>());
                for (int x = 0; x < gridMap[y].Length; x++)
                {
                    ENodeStates val = gridMap[y][x] == '#' ? ENodeStates.eInfected : ENodeStates.eClean;
                    InfectionMap[y].Add(x, val);
                }
            }
            direction = EDirections.eUp;
            currentYPos = InfectionMap.Count / 2;
            currentXPos = InfectionMap[0].Count / 2;
            InfectionBursts = 0;
        }

        public void Move()
        {
            if (InfectionMap[currentYPos][currentXPos] == ENodeStates.eInfected)
            {
                switch (direction)
                {
                    case EDirections.eUp:
                        direction = EDirections.eRight;
                        break;
                    case EDirections.eDown:
                        direction = EDirections.eLeft;
                        break;
                    case EDirections.eLeft:
                        direction = EDirections.eUp;
                        break;
                    case EDirections.eRight:
                        direction = EDirections.eDown;
                        break;
                }
                InfectionMap[currentYPos][currentXPos] = ENodeStates.eFlagged;
            }
            else if (InfectionMap[currentYPos][currentXPos] == ENodeStates.eClean)
            {
                switch (direction)
                {
                    case EDirections.eUp:
                        direction = EDirections.eLeft;
                        break;
                    case EDirections.eDown:
                        direction = EDirections.eRight;
                        break;
                    case EDirections.eLeft:
                        direction = EDirections.eDown;
                        break;
                    case EDirections.eRight:
                        direction = EDirections.eUp;
                        break;
                }
                InfectionMap[currentYPos][currentXPos] = ENodeStates.eWeakened;
            }
            else if (InfectionMap[currentYPos][currentXPos] == ENodeStates.eFlagged)
            {
                switch (direction)
                {
                    case EDirections.eUp:
                        direction = EDirections.eDown;
                        break;
                    case EDirections.eDown:
                        direction = EDirections.eUp;
                        break;
                    case EDirections.eLeft:
                        direction = EDirections.eRight;
                        break;
                    case EDirections.eRight:
                        direction = EDirections.eLeft;
                        break;
                }
                InfectionMap[currentYPos][currentXPos] = ENodeStates.eClean;
            }
            else if (InfectionMap[currentYPos][currentXPos] == ENodeStates.eWeakened)
            {
                InfectionMap[currentYPos][currentXPos] = ENodeStates.eInfected;
                InfectionBursts++;
            }

            switch (direction)
            {
                case EDirections.eUp:
                    currentYPos--;
                    break;
                case EDirections.eDown:
                    currentYPos++;
                    break;
                case EDirections.eLeft:
                    currentXPos--;
                    break;
                case EDirections.eRight:
                    currentXPos++;
                    break;
            }

            if (!InfectionMap.ContainsKey(currentYPos))
            {
                InfectionMap.Add(currentYPos, new Dictionary<int, ENodeStates>());
            }
            if (!InfectionMap[currentYPos].ContainsKey(currentXPos))
            {
                InfectionMap[currentYPos].Add(currentXPos, ENodeStates.eClean);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<string> gridMap = new List<string>();
            foreach (string line in InputLoader.LoadByLines("input.txt"))
            {
                gridMap.Add(line);
            }

            VirusCarrierSimple simpleVirus = new VirusCarrierSimple(gridMap);
            VirusCarrierEnhanced enhancedVirus = new VirusCarrierEnhanced(gridMap);

            for (int i = 0; i < 10000; i++)
            {
                simpleVirus.Move();
            }

            for (int i = 0; i < 10000000; i++)
            {
                enhancedVirus.Move();
            }

            Console.WriteLine(string.Format("Infection Bursts: {0}", simpleVirus.InfectionBursts));
            Console.WriteLine(string.Format("Infection Bursts: {0}", enhancedVirus.InfectionBursts));
            Console.ReadLine();
        }
    }
}
