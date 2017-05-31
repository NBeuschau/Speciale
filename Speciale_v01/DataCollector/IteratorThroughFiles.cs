using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector
{
    class IteratorThroughFiles
    {
        private Dictionary<string, FileObject> savedObjects = new Dictionary<string, FileObject>();
        public Dictionary<string, FileObject> iterator(string path)
        {
            string[] filesInDirectory = null;

            //Tries to see if the directory is locked
            try
            {
                filesInDirectory = Directory.GetFiles(path);
            }
            catch (Exception)
            {
                return savedObjects;
            }


            //Hashes every file in the directory
            foreach (string file in filesInDirectory)
            {
                Console.WriteLine(file);

                    FileObject temp = new FileObject();
                    temp.Size = new FileInfo(file).Length.ToString();
                    temp.Path = file;
                    temp.DateCreated = new FileInfo(file).CreationTime.ToString("dd/MM/yyyy HH:mm:ss.fff");
                    temp.LastModified = new FileInfo(file).LastWriteTime.ToString("dd/MM/yyyy HH:mm:ss.fff");
                    temp.LastAccessed = new FileInfo(file).LastAccessTime.ToString("dd/MM/yyyy HH:mm:ss.fff");

                    savedObjects.Add(file, temp);
            }

            //Get every subdirectory in the given path
            var subDirectories = Directory.GetDirectories(path);

            //Iterates though the subdirectories
            foreach (var directory in subDirectories)
            {
                //Creates a string with the name of the subdirectory only
                string dirName = new DirectoryInfo(directory).Name;

                //Calls the function itself for every subdirectory
                iterator(path + "\\" + dirName);
            }
            return savedObjects;
        }
    }
}
