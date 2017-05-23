using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Shannon10POC.ShannonLogger
{
    class Hasher
    {
        private Dictionary<string, string> hashedFiles = new Dictionary<string, string>();
        public Dictionary<string, string> fileHasher(string path)
        {
            string[] filesInDirectory = null;
            Console.WriteLine(path);
            if (path.Contains("Data"))
            {
                return hashedFiles;
            }
            try
            {
                filesInDirectory = Directory.GetFiles(path);
            }
            catch (Exception)
            {
                Console.WriteLine("Cannot get files in directory for: " + path);
                return hashedFiles;
            }


            foreach (string file in filesInDirectory)
            {
                try
                {
                    hashedFiles.Add(file, md5Hasher(file));
                }
                catch (Exception)
                {
                    Console.WriteLine("Cannot hash file: " + file);
                }
            }

            //Get every subdirectory in the given path
            var subDirectories = Directory.GetDirectories(path);

            //Iterates though the subdirectories
            foreach (var directory in subDirectories)
            {
                //Creates a string with the name of the subdirectory only
                string dirName = new DirectoryInfo(directory).Name;

                //Calls the function itself for every subdirectory
                fileHasher(path + "\\" + dirName);
            }
            return hashedFiles;
        }

        private string md5Hasher(string path)
        {
            using (var md5 = MD5.Create())
            {
                try
                {
                    using (var stream = File.OpenRead(path))
                    {
                        return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to hash file: " + path);
                    return "I cannot change";
                }
            }
        }
    }
}
