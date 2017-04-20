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
        static string PATH = @"C:\Users\PoC-tester";
        static string BACKINGNAME = "backingFromProcMon";
        static string ProcMonPath = @"C:\procmon\Procmon.exe";

        static void Main(string[] args)
        {
            //honeyPotFileMonDetection();

            Logger.getPoCRansomware();
            Thread.Sleep(100000);
        }

        public static void honeyPotFileMonDetection()
        {

            Logger.getPoCRansomware();

            ActionTaker.setBackingName(BACKINGNAME);
            ActionTaker.setPATH(PATH);

            ProcMon.setPathToProcMon(ProcMonPath);

            var t = new Thread(() => ProcMon.createProcmonBackingFile(PATH, BACKINGNAME));
            t.Start();

            FileMon.createFileWatcher(PATH);

            
            Console.WriteLine(Logger.getNAMEONTEST());
            Logger.LogWriter(PATH);
            Logger.postPoCPosted();

            Logger.postPoCTested();
        }
    }
}
