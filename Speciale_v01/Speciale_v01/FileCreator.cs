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
        //This function runs recursively through every subfolder in the given path
        public static void CreateFileInEveryFolder(string path)
        {
            Console.WriteLine(path);
            //Runs the function CreateFile in the path
            CreateFile(path);

            //Get every subdirectory in the given path
            var subDirectories = Directory.GetDirectories(path);

            //Iterates though the subdirectories
            foreach (var directory in subDirectories)
            {
                //Creates a string with the name of the subdirectory only
                string dirName = new DirectoryInfo(directory).Name;
                
                //Calls the function itself for every subdirectory
                CreateFileInEveryFolder(path + "\\" + dirName);
            }
        }

        public static void CreateFile(string path)
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