using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Speciale_v01.TestEnvironmentLogger
{
    class QTHasher
    {
        private Dictionary<string, string> hashedFiles = new Dictionary<string, string>();
        public Dictionary<string, string> fileHasher(string path)
        {
            if (path.Equals(@"C:\Users\Niels Beuschau\AppData")
                || path.Equals(@"C:\Users\Niels Beuschau\Application Data")
                || path.Equals(@"C:\Users\Niels Beuschau\Cookies")
                || path.Equals(@"C:\Users\Niels Beuschau\Documents\My Music")
                || path.Equals(@"C:\Users\Niels Beuschau\Documents\My Pictures")
                || path.Equals(@"C:\Users\Niels Beuschau\Documents\My Videos")
                || path.Equals(@"C:\Users\Niels Beuschau\Local Settings")
                || path.Equals(@"C:\Users\Niels Beuschau\My Documents")
                || path.Equals(@"C:\Users\Niels Beuschau\NetHood")
                || path.Equals(@"C:\Users\Niels Beuschau\PrintHood")
                || path.Equals(@"C:\Users\Niels Beuschau\Anaconda")
                || path.Equals(@"C:\Users\Niels Beuschau\Anaconda3")
                || path.Equals(@"C:\Users\Niels Beuschau\Recent")
                || path.Equals(@"C:\Users\Niels Beuschau\SendTo")
                || path.Equals(@"C:\Users\Niels Beuschau\Start Menu"))
            {
                return hashedFiles;
            }
            string[] filesInDirectory = null;
            try
            {
                filesInDirectory = Directory.GetFiles(path);
            }
            catch (Exception)
            {
                return hashedFiles;
            }

            foreach (string file in filesInDirectory)
            {
                hashedFiles.Add(file, md5Hasher(file));
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
            if(path.Length > 248)
            {
                return "";
            }
            try
            {
                using (var md5 = MD5.Create())
                {
                   FileStream stream = null;
                   try
                   {
                        stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                        stream.Close();
                   }
                   catch (IOException e)
                   {
                       Console.WriteLine(e.StackTrace);
                       return "I cannot change";
                   }
                    try
                    {
                        using (stream = File.OpenRead(path))
                        {
                            return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
                        }
                    }
                    catch
                    {
                        return "Buh!";
                    }
                }
            }
            catch (IOException)
            {

                throw;
            }

        }
    }
}
