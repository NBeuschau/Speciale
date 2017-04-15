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
        static string PATH = @"C:\Users\viruseater1";
        static string BACKINGNAME = "backingFromProcMon";
        static string ProcMonPath = "";

        static void Main(string[] args)
        {

        }

        public void honeyPotFileMonDetection()
        {

            ActionTaker.setBackingName(BACKINGNAME);
            ActionTaker.setPATH(PATH);

            ProcMon.setPathToProcMon(ProcMonPath);

            var t = new Thread(() => ProcMon.createProcmonBackingFile(PATH, BACKINGNAME));
            t.Start();

            FileMon.createFileWatcher(PATH);

            
        }
    }
}
