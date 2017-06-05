using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCollector
{
    class VirusFileParser
    {
        public static List<string> parseTxtToList(string path)
        {

            StreamReader sr = new StreamReader(path);
            List<string> listOfTxtFile = new List<string>();
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                listOfTxtFile.Add(line);
            }
            return listOfTxtFile;
        }
    }
}
