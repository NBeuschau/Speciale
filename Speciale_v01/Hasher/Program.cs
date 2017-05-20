using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hasher
{
    class Program
    {
        static string path1 = @"C:\Users\PoC3\Desktop";
        static string path2 = @"C:\Users\PoC3\Documents";
        static string path3 = @"C:\Users\PoC3\Downloads";
        static string path4 = @"C:\Users\PoC3\Videos";
        static string hashedFilePath = @"C:\Software\";

        static void Main(string[] args)
        {
            Dictionary<string,string> temp = hashingProcess();
            FileWriter.hashedFileLogCreator(hashedFilePath, temp);
        }




        static Dictionary<string, string> hashingProcess()
        {

            //Creates the dictionaries for the hashed files at the end
            Dictionary<string, string> collectedHashFiles = new Dictionary<string, string>();
            Dictionary<string, string> hashedFiles1 = new Dictionary<string, string>();
            Dictionary<string, string> hashedFiles2 = new Dictionary<string, string>();
            Dictionary<string, string> hashedFiles3 = new Dictionary<string, string>();
            Dictionary<string, string> hashedFiles4 = new Dictionary<string, string>();

            //Hashes the files and adds them to the dictionaries
            HashingOfFileSystem tempHasher1 = new HashingOfFileSystem();
            hashedFiles1 = tempHasher1.fileHasher(path1);

            HashingOfFileSystem tempHasher2 = new HashingOfFileSystem();
            hashedFiles2 = tempHasher2.fileHasher(path2);

            HashingOfFileSystem tempHasher3 = new HashingOfFileSystem();
            hashedFiles3 = tempHasher3.fileHasher(path3);

            HashingOfFileSystem tempHasher4 = new HashingOfFileSystem();
            hashedFiles4 = tempHasher4.fileHasher(path4);


            //Adds all dictonaries to a single one.
            hashedFiles1.ToList().ForEach(x => collectedHashFiles.Add(x.Key, x.Value));
            hashedFiles2.ToList().ForEach(x => collectedHashFiles.Add(x.Key, x.Value));
            hashedFiles3.ToList().ForEach(x => collectedHashFiles.Add(x.Key, x.Value));
            hashedFiles4.ToList().ForEach(x => collectedHashFiles.Add(x.Key, x.Value));


            return collectedHashFiles;
        }
    }
}
