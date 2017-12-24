using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelpers;

namespace Day24
{
    class BridgePart
    {
        public List<int> PortStrength { get; set; }

        public BridgePart(string bp)
        {
            string[] s = bp.Split('/');
            PortStrength = new List<int>();
            PortStrength.Add(int.Parse(s[0]));
            PortStrength.Add(int.Parse(s[1]));
        }
    }

    class Program
    {
        static void buildBridge(List<BridgePart> bridgeParts)
        {
            int nextConnector = 0;
            List<BridgePart> bps = new List<BridgePart>(bridgeParts);
            List<BridgePart> bridge = new List<BridgePart>();
            for (int i = 0; i < bps.Count; i++)
            {
                if (bps[i].PortStrength.Contains(nextConnector))
                {
                    bridge.Add(bps[i]);
                    if (bps[i].PortStrength[0] == nextConnector)
                    {
                        nextConnector = bps[i].PortStrength[1];
                    }
                    else
                    {
                        nextConnector = bps[i].PortStrength[0];
                    }
                    bps.Remove(bps[i]);
                    i = -1;
                }
            }
        }

        static void createBridges(List<List<BridgePart>> bridges, List<BridgePart> currentBridge, List<BridgePart> bridgeParts, int nextConnector)
        {
            List<BridgePart> bps = new List<BridgePart>(bridgeParts);
            int next;

            for (int i = 0; i < bps.Count; i++)
            {
                if (bps[i].PortStrength.Contains(nextConnector))
                {
                    if (bps[i].PortStrength[0] == nextConnector)
                    {
                        next = bps[i].PortStrength[1];
                    }
                    else
                    {
                        next = bps[i].PortStrength[0];
                    }
                    List<BridgePart> curBridge = new List<BridgePart>(currentBridge);
                    curBridge.Add(bps[i]);
                    List<BridgePart> remainingParts = new List<BridgePart>(bps);
                    remainingParts.Remove(bps[i]);
                    bridges.Add(curBridge);
                    createBridges(bridges, curBridge, remainingParts, next);
                }
            }
        }

        static void Main(string[] args)
        {
            List<BridgePart> bridgeParts = new List<BridgePart>();
            List<List<BridgePart>> bridges = new List<List<BridgePart>>();
            int bestBridgeStrength = int.MinValue;
            int bestBridgeStrengthLongest = int.MinValue;
            int longestBridge = int.MinValue;

            foreach (string line in InputLoader.LoadByLines("input.txt"))
            {
                bridgeParts.Add(new BridgePart(line));
            }

            createBridges(bridges, new List<BridgePart>(), bridgeParts, 0);

            foreach (List<BridgePart> bps in bridges)
            {
                int strength = 0;
                foreach(BridgePart bp in bps)
                {
                    strength += bp.PortStrength[0];
                    strength += bp.PortStrength[1];
                }
                if (strength > bestBridgeStrength)
                {
                    bestBridgeStrength = strength;
                }
                if (bps.Count > longestBridge)
                {
                    longestBridge = bps.Count;
                    bestBridgeStrengthLongest = strength;
                }
                else if (bps.Count == longestBridge)
                {
                    if (strength > bestBridgeStrengthLongest)
                    {
                        bestBridgeStrengthLongest = strength;
                    }
                }
            }

            Console.WriteLine(string.Format("Strongest bridge strength: {0}", bestBridgeStrength));
            Console.WriteLine(string.Format("Strongest bridge strength of longest bridge: {0}", bestBridgeStrengthLongest));
            Console.ReadLine();
        }
    }
}
