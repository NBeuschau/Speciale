using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HoneyPotPOC
{
    class ActionTaker
    {

        static string pathToBackingFile = "";
        static int INDEXER = 0;
        static string BACKINGNAME = "";
        static HashSet<int> pID = new HashSet<int>();
        static string NAMEONTEST = "";
        static List<string> killedProcesses = new List<string>();

        public static void honeypotChange(string path)
        {
            Thread.Sleep(2000);
            ProcMon.procmonTerminator();
            Thread.Sleep(10000);

            INDEXER++;
            var cpmbf = new Thread(() => ProcMon.createProcmonBackingFile(pathToBackingFile, BACKINGNAME + INDEXER));
            cpmbf.Start();

            ProcMon.convertPMLfileToCSV(pathToBackingFile, BACKINGNAME + (INDEXER - 1) + ".PML", "convertedFile" + (INDEXER - 1) + ".CSV");

            List<CSVfileHandler> parsedData = CSVfileHandler.CSVparser(pathToBackingFile + "convertedFile" + (INDEXER - 1) + ".CSV");

            foreach (var item in parsedData)
            {
                if (!item.processName.Equals("Explorer.EXE"))
                {
                    try
                    {
                        Console.WriteLine("Process :" + item.processName + " has changed a honeypot");
                        Console.WriteLine("It has process ID: " + item.PID + "\n");
                        pID.Add(item.PID);
                    }
                    catch
                    {

                    }
                }
            }

            Console.WriteLine("Process: " + Process.GetProcessById(pID.Last()).ProcessName + " is killed due to suspicious behaviour");
            //Console.WriteLine("Do you wish to kill? ");
            //string killInput = Console.ReadLine();
            Thread.Sleep(3000);
            killedProcesses.Add(Process.GetProcessById(pID.Last()).ProcessName);
            killProcess(pID.Last());


            //Dataanalysis
        }

        private static void killProcess(int PID)
        {
            var process = Process.GetProcessById(PID);
            process.Kill();
            process.WaitForExit();
        }

        public static void setPathToBackingFile(string path)
        {
            pathToBackingFile = path;
        }

        public static void setBackingName(string name)
        {
            BACKINGNAME = name;
        }

        public static List<string> getKilledProcesses()
        {
            return killedProcesses;
        }
    }
}
