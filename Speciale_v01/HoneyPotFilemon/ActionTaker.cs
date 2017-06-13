using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        private static Boolean killedFirstProcess = false;
        private static DateTime firstKilledProcessTime = new DateTime();

        //A change has been registered to a honeypot
        public static void honeypotChange(string path)
        {
            //Shut down procmon in order to get logfile
            ProcMon.procmonTerminator(pathToBackingFile, BACKINGNAME + INDEXER);

            INDEXER++;
            //Start up procmon with a new backingfile
            var cpmbf = new Thread(() => ProcMon.createProcmonBackingFile(pathToBackingFile, BACKINGNAME + INDEXER));
            cpmbf.Start();

            Thread.Sleep(3000);

            //Convert the PMLfile to CSV
            ProcMon.convertPMLfileToCSV(pathToBackingFile, BACKINGNAME + (INDEXER - 1) + ".PML", "convertedFile" + (INDEXER - 1) + ".CSV");

            bool hasCSVbeenWritten = false;
            Console.WriteLine("Path to CSV file: " + pathToBackingFile + "\\" + "convertedFile" + (INDEXER - 1) + ".CSV");

            //Wait for the conversion to be completed
            while (hasCSVbeenWritten == false)
            {
                try
                {
                    using (Stream stream = new FileStream(pathToBackingFile + "\\" + "convertedFile" + (INDEXER - 1) + ".CSV", FileMode.Open))
                    {
                        hasCSVbeenWritten = true;
                        stream.Dispose();
                    }
                }
                catch (IOException)
                {

                }
                Thread.Sleep(50);
            }
            //Parse the CSVfile
            List<CSVfileHandler> parsedData = CSVfileHandler.CSVparser(pathToBackingFile + "\\" + "convertedFile" + (INDEXER - 1) + ".CSV");

            //Kill every process that has touched a honeypot
            foreach (var item in parsedData)
            {
                if (!item.processName.Equals("Explorer.EXE") || !item.processName.Equals("HoneyPotFilemon.exe"))
                {
                    try
                    {
                        pID.Add(item.PID);
                        killedProcesses.Add(Process.GetProcessById(item.PID).ProcessName);
                        try
                        {
                            Console.WriteLine("Process: " + Process.GetProcessById(item.PID).ProcessName + " is killed due to suspicious behaviour");
                            killProcess(item.PID);
                        }
                        catch (Exception)
                        {
                            //Save processname as a temp
                            Console.WriteLine("Killing of the process failed");
                        }
                    }
                    catch
                    {

                    }
                }
            }

            if (!killedFirstProcess)
            {
                firstKilledProcessTime = DateTime.Now;
                killedFirstProcess = true;
            }
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

        public static DateTime getFirstKilledTime()
        {
            return firstKilledProcessTime;
        }

        public static void terminateProcmon()
        {
            ProcMon.procmonTerminator(pathToBackingFile, BACKINGNAME + INDEXER);
        }
    }
}
