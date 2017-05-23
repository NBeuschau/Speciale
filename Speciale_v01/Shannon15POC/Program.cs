using Shannon15POC.ShannonLogger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shannon15POC
{
    class Program
    {
        //Remember to change path in filemon as well
        private static string path1 = @"C:\Users\PoC3\Desktop";
        private static string path2 = @"C:\Users\PoC3\Documents";
        private static string path3 = @"C:\Users\PoC3\Downloads";
        private static string path4 = @"C:\Users\PoC3\Videos";

        static string PATH = @"C:\Users\PoC3";
        static string BACKINGNAME = "backingFromProcMon";
        static string pathToBackingFile = @"C:\procmon\backingFileTest";
        static string ProcMonPath = @"C:\procmon\Procmon.exe";

        static string RANSOMWAREDOWNLOADERPATH = @"C:\Software\Shannon15RansomwareDownloader\bin\Release\Shannon15RansomwareDownloader.exe";

        static double entropyThreshold = 0.9;
        static int thresholdToReaction = 14;
        static int secondsInThreshold = 60;

        static void Main(string[] args)
        {
            //string path = @"C:\speciale";
            /*
            string path = @"C:\Program Files (x86)\Hearthstone\Data\Win\spells0.unity3d";
            
            FileInfo fil = new FileInfo(path);
            ShannonEntropy test = new ShannonEntropy();

            Console.WriteLine(test.CalculateEntropy(fil));
                /*
            EntropyHandler nonStatic = new EntropyHandler();
            Dictionary<string, double> temp = nonStatic.getEntropyOfAllFilesInPath(path);

            foreach (var item in temp)
            {
                Console.WriteLine(item.Key + " - " + item.Value);
            }
            */
            // FileMon.CreateFileWatcher(path);
            //Console.WriteLine(FilemonEventHandler.returnFilePath(path));
            Thread.Sleep(30000);

            shannonEntropyFileMonDetection();

        }

        public static void shannonEntropyFileMonDetection()
        {
            Logger.getPoCRansomware();

            Thread.Sleep(1000);

            Logger.postPoCFetched();

            while (!Logger.getHasFetched())
            {
                Thread.Sleep(500);
            }

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
            //TODO fix call to server such that it is not honeypotpoc that is called
            Logger.LogWriter(PATH);

            Logger.postPoCTested();
            Logger.postPoCPosted();

            Thread.Sleep(30000);

        }
    }
}
