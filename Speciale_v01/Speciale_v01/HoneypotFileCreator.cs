using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Speciale_v01
{
    class FileCreator
    {
        public void CreateFileInEveryFolder(string path)
        {
            Console.WriteLine(path);
            CreateFile(path);

            var directories = Directory.GetDirectories(path);
            foreach (var directory in directories)
            {
                string dirName = new DirectoryInfo(directory).Name;
                CreateFileInEveryFolder(path + "\\" + dirName);
            }
        }

        public void CreateFile(string path)
        {
            //Create a number of files based on the size of the folder
            //Create random strings followed by honeypot and then the type of file

            string filePath = @path + "\\MyTest_v01_honeypot.txt";
            if (!File.Exists(filePath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine("HelloWorld");
                    sw.WriteLine("Næste skridt er at slette lortet igen! :D");
                    sw.WriteLine("Og så lave andet end kun txt filer");
                }
            }
        }
    }
}


/*TextWriter tw = new StreamWriter(path, true);
tw.WriteLine("The next line!");
    tw.Close(); */