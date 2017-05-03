using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShannonPOC
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\speciale";
            /*
            string path = @"C:\speciale\test\shannon\bitch1.7z";
            FileInfo fil = new FileInfo(path);

            Console.WriteLine(ShannonEntropy.CalculateEntropy(fil));
            EntropyHandler nonStatic = new EntropyHandler();
            Dictionary<string, double> temp = nonStatic.getEntropyOfAllFilesInPath(path);

            foreach (var item in temp)
            {
                Console.WriteLine(item.Key + " - " + item.Value);
            }
            */
            FileMon.CreateFileWatcher(path);

            Console.ReadLine();
        }
    }
}
