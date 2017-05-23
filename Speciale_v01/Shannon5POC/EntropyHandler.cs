using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shannon5POC
{
    class EntropyHandler
    {
        static string path1 = @"C:\";
        static string path2 = @"C:\";
        static string path3 = @"C:\";
        static string path4 = @"C:\";
        static int thresholdNum = 10;
        static double shannonThreshold = 0.4;


        static Dictionary<string, double> entropiesOfFiles = new Dictionary<string, double>();
        private static List<DateTime> threshold = new List<DateTime>();


        public static void entropyCollector()
        {
            //Takes the entropy for each of the four directories and adds that to a single list.
            ShannonEntropy tempEntropyCalculator1 = new ShannonEntropy();
            tempEntropyCalculator1.getEntropyOfAllFilesInPath(path1).ToList().ForEach(x => entropiesOfFiles.Add(x.Key, x.Value));

            ShannonEntropy tempEntropyCalculator2 = new ShannonEntropy();
            tempEntropyCalculator2.getEntropyOfAllFilesInPath(path2).ToList().ForEach(x => entropiesOfFiles.Add(x.Key, x.Value));

            ShannonEntropy tempEntropyCalculator3 = new ShannonEntropy();
            tempEntropyCalculator3.getEntropyOfAllFilesInPath(path3).ToList().ForEach(x => entropiesOfFiles.Add(x.Key, x.Value));

            ShannonEntropy tempEntropyCalculator4 = new ShannonEntropy();
            tempEntropyCalculator4.getEntropyOfAllFilesInPath(path4).ToList().ForEach(x => entropiesOfFiles.Add(x.Key, x.Value));


            //TODO DOWNLOAD RANSOMWARE IF THE LOGGER IS READY AS WELL

        }

        public static void changeDetectedInFile(string path)
        {
            ShannonEntropy tempEntropyCalculator = new ShannonEntropy();
            //TODO what if the file doesn't exists anymore

            FileInfo tempFileInf = new FileInfo(path);

            double changedFileEntropy = tempEntropyCalculator.CalculateEntropy(tempFileInf);

            Console.WriteLine("File " + path + " has been changed to and has now and entropy of " + changedFileEntropy);
            if(changedFileEntropy == -1)
            {
                return;
            }

            List<DateTime> temp = new List<DateTime>();
            if ((changedFileEntropy - entropiesOfFiles[path]) > shannonThreshold)
            {
                DateTime now = DateTime.Now;
                foreach (DateTime t in threshold)
                {
                    if(600 < now.Subtract(t).Seconds){
                        temp.Add(t);
                    }
                }

                foreach (DateTime t in temp)
                {
                    threshold.Remove(t);
                }

                if(threshold.Count > thresholdNum)
                {
                    //ALERT!
                }
            }
        }


        public static void renameDetectedInFile(string pathOld, string pathNew)
        {
            ShannonEntropy tempEntropyCalculator = new ShannonEntropy();
            //Hvad gør vi hvis filen ikke eksisterer længere?


            FileInfo tempFileInf = new FileInfo(pathNew);

            double changedFileEntropy = tempEntropyCalculator.CalculateEntropy(tempFileInf);


            List<DateTime> temp = new List<DateTime>();
            if ((changedFileEntropy - entropiesOfFiles[pathOld]) > shannonThreshold)
            {
                DateTime now = DateTime.Now;
                foreach (DateTime t in threshold)
                {
                    if (600 < now.Subtract(t).Seconds)
                    {
                        temp.Add(t);
                    }
                }

                foreach (DateTime t in temp)
                {
                    threshold.Remove(t);
                }

                if (threshold.Count > thresholdNum)
                {
                    //ALERT!
                }
            }
        }




    }
}


/*
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


    */