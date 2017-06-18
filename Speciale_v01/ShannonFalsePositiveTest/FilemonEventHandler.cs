using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShannonFalsePositiveTest
{
    class FilemonEventHandler
    {

        private static DateTime firstDetected;
        static Boolean hasMadeFirstDetection = false;
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
            Double originalFileEntropy = 0.0;

            Console.WriteLine("File " + e.FullPath + " has been changed to and has now an entropy of " + changedFileEntropy);
            if (changedFileEntropy == -1)
            {
                return;
            }

            try
            {
                originalFileEntropy = savedEntropies[e.FullPath];
            }
            catch (Exception)
            {

            }

            entropyHandler(e, originalFileEntropy, changedFileEntropy);
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


            Console.WriteLine("File " + e.FullPath + " has been created and entropy is now " + createdFileEntropy);
            if (createdFileEntropy == -1)
            {
                return;
            }

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
                ShannonEntropy.removeKeyFromSavedEntropies(oldFilePath);
                ShannonEntropy.addKeyAndDoubleToSavedEntropies(e.FullPath, createdFileEntropy);
                if(createdFileEntropy > entropyThreshold)
                {
                    react(e);
                }
            }
        }

        internal static void deletionOccured(FileSystemEventArgs e)
        {
            string[] filesInDirectory = null;

            filesInDirectory = Directory.GetFiles(returnFilePath(e.FullPath));

            Boolean newSimilarFileIsCreated = false;

            ShannonEntropy entropyCreator = new ShannonEntropy();

            string fileName = returnFileName(e.FullPath);

            double oldEntropy = ShannonEntropy.getSavedEntropies()[e.FullPath];
            foreach (string s in filesInDirectory)
            {
                if (s.Contains(fileName))
                {
                    newSimilarFileIsCreated = true;
                    FileInfo newFileInfo = new FileInfo(s);
                    double newEntropy = entropyCreator.CalculateEntropy(newFileInfo);

                    //TODO  react if needed
                    entropyHandler(e, oldEntropy, newEntropy);
                }
            }

            ShannonEntropy.removeKeyFromSavedEntropies(e.FullPath);
        }

        private static void react(FileSystemEventArgs e)
        {
            threshold.Add(DateTime.Now);
            List<DateTime> temp = new List<DateTime>();
            DateTime now = DateTime.Now;

            foreach (DateTime t in threshold)
            {
                if (secondsInThreshold < (now.Subtract(t).Seconds))
                {
                    temp.Add(t);
                }
            }

            foreach (DateTime t in temp)
            {
                threshold.Remove(t);
            }
            Console.WriteLine("A suspicious activity has been found. Threshold is: " + threshold);
            if (threshold.Count > thresholdToReaction)
            {
                if (!hasMadeFirstDetection)
                {
                    hasMadeFirstDetection = true;
                    firstDetected = DateTime.Now;
                }
                Console.WriteLine("File: " + e.FullPath + " has been " + e.ChangeType + " and the responsible process will now pay the ultimate price!");
            }
        }

        private static void entropyHandler(FileSystemEventArgs e, double originalFileEntropy, double newFileEntropy)
        {
            if(originalFileEntropy < 0.1)
            {
               if(newFileEntropy > 0.6)
                {
                    react(e);
                }
            }
            else if (originalFileEntropy < 0.2)
            {
                if(newFileEntropy > 0.65)
                {
                    react(e);
                }
            }
            else if (originalFileEntropy < 0.3)
            {
                if (newFileEntropy > 0.65)
                {
                    react(e);
                }
            }
            else if (originalFileEntropy < 0.4)
            {
                if (newFileEntropy > 0.7)
                {
                    react(e);
                }
            }
            else if (originalFileEntropy < 0.5)
            {
                if (newFileEntropy > 0.7)
                {
                    react(e);
                }
            }
            else if (originalFileEntropy < 0.6)
            {
                if (newFileEntropy > 0.8)
                {
                    react(e);
                }
            }
            else if (originalFileEntropy < 0.7)
            {
                if (newFileEntropy > 0.8)
                {
                    react(e);
                }
            }
            else if (originalFileEntropy < 0.8)
            {
                if (newFileEntropy > 0.85)
                {
                    react(e);
                }
            }
            else if (originalFileEntropy < 0.9)
            {
                if (newFileEntropy > 0.95)
                {
                    react(e);
                }
            }
            else if (originalFileEntropy < 0.91)
            {
                if (newFileEntropy > 0.97)
                {
                    react(e);
                }
            }
            else if (originalFileEntropy < 0.92)
            {
                if (newFileEntropy > 0.97)
                {
                    react(e);
                }
            }
            else if (originalFileEntropy < 0.93)
            {
                if (newFileEntropy > 0.975)
                {
                    react(e);
                }
            }
            else if (originalFileEntropy < 0.94)
            {
                if (newFileEntropy > 0.98)
                {
                    react(e);
                }
            }
            else if (originalFileEntropy < 0.95)
            {
                if (newFileEntropy > 0.98)
                {
                    react(e);
                }
            }
            else if (originalFileEntropy < 0.96)
            {
                if (newFileEntropy > 0.985)
                {
                    react(e);
                }
            }
            else if (originalFileEntropy < 0.97)
            {
                if (newFileEntropy > 0.99)
                {
                    react(e);
                }
            }
            else if (originalFileEntropy < 0.98)
            {
                if (newFileEntropy > 0.99)
                {
                    react(e);
                }
            }
            else if (originalFileEntropy < 0.99)
            {
                if (newFileEntropy > 0.995)
                {
                    react(e);
                }
            }
            else if (originalFileEntropy < 0.999)
            {
                if (newFileEntropy > 0.9992)
                {
                    react(e);
                }
            }
            else if (originalFileEntropy < 0.9999)
            {
                if (newFileEntropy > 0.9999)
                {
                    react(e);
                }
            }
            else if (originalFileEntropy < 1)
            {
                if (newFileEntropy > 0.99995)
                {
                    react(e);
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
        public static void setFirstDetected()
        {
            firstDetected = DateTime.Now;
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
