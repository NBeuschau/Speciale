using HoneyPot5POC.PocLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HoneyPot5POC
{
    class Program
    {
        //In addition, four paths needs to be set in PocLogger\Logger
        //static string PATH = @"C:\Users\PoC";
        static string PATH = @"C:\Users\PoC3";
        static string BACKINGNAME = "backingFromProcMon";
        static string pathToBackingFile = @"C:\procmon\backingFileTest";
        static string ProcMonPath = @"C:\procmon\Procmon.exe";

        //Path to ransomware downloader
        static string RANSOMWAREDOWNLOADERPATH = @"C:\Software\HoneyPot5POCRansomwareDownloader\bin\Release\HoneyPot5POCRansomwareDownloader.exe";

        static void Main(string[] args)
        {
            Thread.Sleep(30000);
            honeyPotFileMonDetection();
        }

        public static void honeyPotFileMonDetection()
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
            BACKINGNAME = BACKINGNAME + 0;
            var t = new Thread(() => ProcMon.createProcmonBackingFile(pathToBackingFile, BACKINGNAME));
            t.Start();

            Console.WriteLine(Logger.getNAMEONTEST());
            Logger.LogWriter(PATH);
            Logger.postPoCTested();
            Logger.postPoCPosted();

            Thread.Sleep(30000);
        }
    }
}
