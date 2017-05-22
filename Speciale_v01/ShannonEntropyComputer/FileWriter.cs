using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShannonEntropyComputer
{
    class FileWriter
    {
        public static void listToTxt(string PATH, List<string> temp)
        {
            string filePath = PATH + "\\textFile1.txt";
            if (!File.Exists(filePath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    foreach (var item in temp)
                    {
                        sw.WriteLine(item);
                    }
                }
            }
        }
    }
}
