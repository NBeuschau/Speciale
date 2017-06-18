using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShannonFalsePositiveTest
{
    class Program
    {
        //Set path for folders
        private static string path1 = @"C:\Users\Baseline\Desktop";
        private static string path2 = @"C:\Users\Baseline\Documents";
        private static string path3 = @"C:\Users\Baseline\Downloads";
        private static string path4 = @"C:\Users\Baseline\Videos";

        //Set path for procmon
        static string PATH = @"C:\Users\Baseline";

        //Set threshold and duration
        static double entropyThreshold = 0.9;
        static int thresholdToReaction = 2;
        static int secondsInThreshold = 60;

        static void Main(string[] args)
        {
            //Wait for program to start
            Thread.Sleep(30000);

            shannonEntropyFileMonDetection();

        }

        public static void shannonEntropyFileMonDetection()
        {
            FilemonEventHandler.setEntropyThreshold(entropyThreshold);
            FilemonEventHandler.setThresholdToReaction(thresholdToReaction);
            FilemonEventHandler.setSecondsInThreshold(secondsInThreshold);


            //Find entropy of all files
            ShannonEntropy temp1 = new ShannonEntropy();
            temp1.getEntropyOfAllFilesInPath(path1);

            ShannonEntropy temp2 = new ShannonEntropy();
            temp2.getEntropyOfAllFilesInPath(path2);

            ShannonEntropy temp3 = new ShannonEntropy();
            temp3.getEntropyOfAllFilesInPath(path3);

            ShannonEntropy temp4 = new ShannonEntropy();
            temp4.getEntropyOfAllFilesInPath(path4);

            //Print the entropies
            Dictionary<string, double> test = ShannonEntropy.getSavedEntropies();
            foreach (var item in test)
            {
                Console.WriteLine(item.Key + " - " + item.Value);
            }



            Thread.Sleep(30000);

        }
    }
}
