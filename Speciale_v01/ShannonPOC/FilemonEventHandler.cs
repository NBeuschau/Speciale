using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShannonPOC
{
    class FilemonEventHandler
    {
        private static DateTime firstDetected;
        private static double entropyThreshold = 0.0;
        private static int thresholdToReaction = 0;
        private static List<DateTime> threshold = new List<DateTime>();
        private static int secondsInThreshold = 0;

        internal static void changeOccured(FileSystemEventArgs e)
        {
            //Kig på entropien før og efter
            Dictionary<string, double> savedEntropies = ShannonEntropy.getSavedEntropies();
            FileInfo changedFile = new FileInfo(e.FullPath);
            ShannonEntropy entropyCalculator = new ShannonEntropy();
            Double changedFileEntropy = entropyCalculator.CalculateEntropy(changedFile);

            Double originalFileEntropy = savedEntropies[e.FullPath];

            //TODO Find ud af threshold
            if(changedFileEntropy-originalFileEntropy > 0.05 && changedFileEntropy > 0.9)
            {
                //React
                threshold.Add(DateTime.Now);
                List<DateTime> temp = new List<DateTime>();
                DateTime now = DateTime.Now;

                foreach (DateTime t in threshold)
                {
                    if ()
                }
            }
            else if (changedFileEntropy > 0.9 && originalFileEntropy < 0.9)
            {
                //React
            }
        }

        internal static void creationOccured(FileSystemEventArgs e)
        {
            //Er der en fil i directoriet der har samme entropi som denne er den blot rykket
            //Løb listen af keys igennem, se value, nogen ens? Godt
            //add til databasen den nye fil, slet den gamle

            Dictionary<string, double> savedEntropies = new Dictionary<string, double>();

            savedEntropies = ShannonEntropy.getSavedEntropies();

            FileInfo createdFileInfo = new FileInfo(e.FullPath);

            ShannonEntropy entropyCreator = new ShannonEntropy();
            double createdFileEntropy = entropyCreator.CalculateEntropy(createdFileInfo);

            Boolean fileHasBeenMoved = false;
            string oldFilePath = "";

            foreach (var item in savedEntropies)
            {
                if(item.Value == createdFileEntropy)
                {
                    //File has been moved
                    fileHasBeenMoved = true;
                    oldFilePath = item.Key;
                }
            }

            if (fileHasBeenMoved)
            {
                ShannonEntropy.removeKeyFromSavedEntropies(oldFilePath);
                ShannonEntropy.addKeyAndDoubleToSavedEntropies(e.FullPath, createdFileEntropy);
            }
            else
            {
                //TODO find threshold på nye filer og om entropien er for høj
            }
        }

        internal static void deletionOccured(FileSystemEventArgs e)
        {
            string[] filesInDirectory = null;

            filesInDirectory = Directory.GetFiles(returnFilePath(e.FullPath));

            Boolean newSimilarFileIsCreated = false;

            ShannonEntropy entropyCreator = new ShannonEntropy();

            string fileName = returnFileName(e.FullPath);

            foreach (string s in filesInDirectory)
            {
                if (s.Contains(fileName))
                {
                    newSimilarFileIsCreated = true;
                    FileInfo newFileInfo = new FileInfo(s);
                    double newEntropy = entropyCreator.CalculateEntropy(newFileInfo);
                    double oldEntropy = ShannonEntropy.getSavedEntropies()[e.FullPath];
                               
                    //TODO  react if needed
                }
            }
        }

        public static string returnFileName(string fullPath)
        {

            int lastSlash = 0;
            int lastDot = 0;
            string fileName = "";

            for (int i = 0; i < fullPath.Length - 1; i++)
            {
                if (fullPath.Substring(i, 1).Equals(@"\"))
                {
                    lastSlash = i;
                }
                if (fullPath.Substring(i, 1).Equals("."))
                {
                    lastDot = i;
                }
            }
            fileName = fullPath.Substring(lastSlash + 1, lastDot - lastSlash - 1);

            return fileName;
        }

        public static string returnFilePath(string fullPath)
        {

            int lastSlash = 0;
            int lastDot = 0;
            string fileName = "";

            for (int i = 0; i < fullPath.Length - 1; i++)
            {
                if (fullPath.Substring(i, 1).Equals(@"\"))
                {
                    lastSlash = i;
                }
            }
            fileName = fullPath.Substring(0,lastSlash + 1);

            return fileName;
        }

        internal static DateTime getFirstDetected()
        {
            return firstDetected;
        }

        public static void setEntropyThreshold(double d)
        {
            entropyThreshold = d;
        }

        public static void setThresholdToReaction(int i)
        {
            thresholdToReaction = i;
        }

        public static void setSecondsInThreshold(int i)
        {
            secondsInThreshold = i;
        }
    }
}
