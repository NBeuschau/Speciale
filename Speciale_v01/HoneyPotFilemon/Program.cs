using HoneyPotPOC.PocLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HoneyPotPOC
{
    class Program
    {
        //In addition, four paths needs to be set in PocLogger\Logger
        //static string PATH = @"C:\Users\PoC";
        static string PATH = @"C:\Users\PoC";
        static string BACKINGNAME = "backingFromProcMon";
        static string pathToBackingFile = @"C:\procmon\backingFileTest";
        static string ProcMonPath = @"C:\procmon\Procmon.exe";

        //Path to ransomware downloader
        static string RANSOMWAREDOWNLOADERPATH = @"C:\Software\HoneyPotPOCRansomwareDownloader\bin\Release\HoneyPotPOCRansomwareDownloader.exe";

        static void Main(string[] args)
        {
            //This wait is made such that a snapshot of the virtual machine could be made during the start of the program.
            Thread.Sleep(30000);
            honeyPotFileMonDetection();
        }

        public static void honeyPotFileMonDetection()
        {
            //Fetch the ransomwarename
            Logger.getPoCRansomware();

            Thread.Sleep(1000);
            //Inform the server that the ransomware has been fetched
            Logger.postPoCFetched();

            //Wait for response from the server
            while (!Logger.getHasFetched())
            {
                Thread.Sleep(500);
            }

            //Sets the correct values in different classes
            Logger.setRansomwareDownloaderPath(RANSOMWAREDOWNLOADERPATH);

            ActionTaker.setBackingName(BACKINGNAME);
            ActionTaker.setPathToBackingFile(pathToBackingFile);

            ProcMon.setPathToProcMon(ProcMonPath);
            BACKINGNAME = BACKINGNAME + 0;

            //Start the procmon
            var t = new Thread(() => ProcMon.createProcmonBackingFile(pathToBackingFile, BACKINGNAME));
            t.Start();
                        
            Console.WriteLine(Logger.getNAMEONTEST());
            //Start the logger
            Logger.LogWriter(PATH);

            //Post that the ransomware succesfully has been tested
            Logger.postPoCTested();

            //Post the tested results
            Logger.postPoCPosted();


            Thread.Sleep(30000);
        }
    }
}
