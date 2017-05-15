using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hasher
{
    class FileWriter
    {
        public static void hashedFileLogCreator(string PATH, Dictionary<string, string> hashedFiles)
        {
            string filePath = PATH + "\\HashedFilesLog.txt";
            if (!File.Exists(filePath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    foreach (var item in hashedFiles)
                    {
                        sw.WriteLine(item.Key + "?" + item.Value);
                    }
                }
            }
        }
    }
}
