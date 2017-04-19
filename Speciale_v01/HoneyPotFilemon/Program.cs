using HoneyPotFilemon.PocLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HoneyPotFilemon
{
    class Program
    {
        //In addition, four paths needs to be set in PocLogger\Logger
        static string PATH = @"C:\Users\viruseater1";
        static string BACKINGNAME = "backingFromProcMon";
        static string ProcMonPath = "";

        static void Main(string[] args)
        {

        }

        public void honeyPotFileMonDetection()
        {

            Logger.getBaseRansomware();

            ActionTaker.setBackingName(BACKINGNAME);
            ActionTaker.setPATH(PATH);

            ProcMon.setPathToProcMon(ProcMonPath);

            var t = new Thread(() => ProcMon.createProcmonBackingFile(PATH, BACKINGNAME));
            t.Start();

            FileMon.createFileWatcher(PATH);

            
            Console.WriteLine(Logger.getNAMEONTEST());
            Logger.LogWriter(PATH);
            Logger.postBasePosted();

            Logger.postBaseTested();
        }
    }
}
