using ShannonPOC.ShannonLogger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShannonPOC
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
        static string BACKINGNAME = "backingFromProcMon";
        static string pathToBackingFile = @"C:\procmon\backingFileTest";
        static string ProcMonPath = @"C:\procmon\Procmon.exe";

        static string RANSOMWAREDOWNLOADERPATH = @"C:\Software\ShannonRansomwareDownloader\bin\Release\ShannonRansomwareDownloader.exe";

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
            //Get name of ransomware
            Logger.getPoCRansomware();

            Thread.Sleep(1000);

            //Post name to server that the ransomware has been fetched
            Logger.postPoCFetched();

            //Wait for the server to respond
            while (!Logger.getHasFetched())
            {
                Thread.Sleep(500);
            }

            //Initialize variables
            Logger.setRansomwareDownloaderPath(RANSOMWAREDOWNLOADERPATH);

            ActionTaker.setBackingName(BACKINGNAME);
            ActionTaker.setPathToBackingFile(pathToBackingFile);

            ProcMon.setPathToProcMon(ProcMonPath);

            FilemonEventHandler.setEntropyThreshold(entropyThreshold);
            FilemonEventHandler.setThresholdToReaction(thresholdToReaction);
            FilemonEventHandler.setSecondsInThreshold(secondsInThreshold);

            Logger.setPath1(path1);
            Logger.setPath2(path2);
            Logger.setPath3(path3);
            Logger.setPath4(path4);
            Logger.setPathFileWatch(PATH);

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


            //Start procmon
            BACKINGNAME = BACKINGNAME + 0;
            var t = new Thread(() => ProcMon.createProcmonBackingFile(pathToBackingFile, BACKINGNAME));
            t.Start();

            //Start filemon
            //When filemon sees a reaction it posts to filemoneventhandler
            //Filemoneventhandler then deems if it is nessesary to take action, using actiontaker
            Console.WriteLine(Logger.getNAMEONTEST());

            //Start logger
            Logger.LogWriter(PATH);

            //Post to server that it has been tested
            Logger.postPoCTested();

            //Post to server the results
            Logger.postPoCPosted();

            Thread.Sleep(30000);

        }
    } 
}
