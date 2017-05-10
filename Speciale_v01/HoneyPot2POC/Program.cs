using HoneyPot2POC.PocLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HoneyPot2POC
{
    class Program
    {
        //In addition, four paths needs to be set in PocLogger\Logger
        //static string PATH = @"C:\Users\PoC";
        static string PATH = @"C:\Users\PoC2";
        static string BACKINGNAME = "backingFromProcMon";
        static string pathToBackingFile = @"C:\procmon\backingFileTest";
        static string ProcMonPath = @"C:\procmon\Procmon.exe";

        //Path to ransomware downloader
        static string RANSOMWAREDOWNLOADERPATH = @"C:\Software\HoneyPotPOCRansomwareDownloader\bin\Release\HoneyPotPOCRansomwareDownloader.exe";

        static void Main(string[] args)
        {
            Hasher hash = new Hasher();
            hash.fileHasher(@"C:\Speciale\Test\shannon\path3");
            Console.ReadLine();
            //honeyPotFileMonDetection();
        }

        public static void honeyPotFileMonDetection()
        {

            Logger.getPoCRansomware();

            Logger.postPoCFetched();

            Logger.setRansomwareDownloaderPath(RANSOMWAREDOWNLOADERPATH);

            ActionTaker.setBackingName(BACKINGNAME);
            ActionTaker.setPathToBackingFile(pathToBackingFile);

            ProcMon.setPathToProcMon(ProcMonPath);
            BACKINGNAME = BACKINGNAME + 0;
            var t = new Thread(() => ProcMon.createProcmonBackingFile(pathToBackingFile, BACKINGNAME));
            t.Start();

            FileMon.createFileWatcher(PATH);


            Console.WriteLine(Logger.getNAMEONTEST());
            Logger.LogWriter(PATH);
            Logger.postPoCPosted();

            Logger.postPoCTested();
        }
    }
}
