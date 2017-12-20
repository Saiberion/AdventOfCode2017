using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoCHelpers
{
    public class InputLoader
    {
        public static List<string> LoadByLines(string filename)
        {
            StreamReader file = new StreamReader(filename);
            string line;
            List<string> lines = new List<string>();

            while ((line = file.ReadLine()) != null)
            {
                lines.Add(line);
            }

            file.Close();
            return lines;
        }
    }
}
