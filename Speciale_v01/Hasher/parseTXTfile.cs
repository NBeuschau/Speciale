using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hasher
{
    class parseTXTfile
    {
        public static void testParseTXTfile(string hashedFilePath)
        {
            string line;
            string[] pairs = new string[2];
            Dictionary<string, string> hashedFilesReturn = new Dictionary<string, string>();
            System.IO.StreamReader file =
                new System.IO.StreamReader(hashedFilePath + "\\HashedFilesLog.txt");
            while ((line = file.ReadLine()) != null)
            {
                pairs = line.Split('?');
                hashedFilesReturn.Add(pairs[0], pairs[1]);
            }
            file.Close();

            foreach (var item in hashedFilesReturn)
            {
                Console.WriteLine(item.Key + " - " + item.Value);
            }


        }
    }
}
