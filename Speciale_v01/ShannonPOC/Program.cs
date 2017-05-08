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
        private static string path1 = "";
        private static string path2 = "";
        private static string path3 = "";
        private static string path4 = "";

        static string PATH = @"C:\Users\PoC";
        static string BACKINGNAME = "backingFromProcMon";
        static string pathToBackingFile = @"C:\procmon\backingFileTest";
        static string ProcMonPath = @"C:\procmon\Procmon.exe";

        static string RANSOMWAREDOWNLOADERPATH = @"C:\Software\HoneyPotPOCRansomwareDownloader\bin\Release\HoneyPotPOCRansomwareDownloader.exe";

        static double entropyThreshold = 0.9;
        static int thresholdToReaction = 5;
        static int secondsInThreshold = 60;

        static void Main(string[] args)
        {
            //string path = @"C:\speciale";
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


            //Start logger
            //TODO fix call to server such that it is not honeypotpoc that is called

            //Start procmon

            //Start filemon
            //When filemon sees a reaction it posts to filemoneventhandler
            //Filemoneventhandler then deems if it is nessesary to take action, using actiontaker

            //Start download

            Console.ReadLine();
        }

        public static void shannonEntropyFileMonDetection()
        {
            Logger.getPoCRansomware();

            Logger.setRansomwareDownloaderPath(RANSOMWAREDOWNLOADERPATH);

            ActionTaker.setBackingName(BACKINGNAME);
            ActionTaker.setPathToBackingFile(pathToBackingFile);

            ProcMon.setPathToProcMon(ProcMonPath);

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

            BACKINGNAME = BACKINGNAME + 0;
            var t = new Thread(() => ProcMon.createProcmonBackingFile(pathToBackingFile, BACKINGNAME));
            t.Start();

            FileMon.CreateFileWatcher(PATH);
            Logger.LogWriter(PATH);
            Logger.postPoCPosted();
            Logger.postPoCTested();

        }
    }
}
