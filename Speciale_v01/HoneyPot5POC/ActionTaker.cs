using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HoneyPot5POC
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

        public static void honeypotChange(string path)
        {
            //Thread.Sleep(2000);
            ProcMon.procmonTerminator(pathToBackingFile, BACKINGNAME+INDEXER);
            //Thread.Sleep(10000);

            INDEXER++;
            var cpmbf = new Thread(() => ProcMon.createProcmonBackingFile(pathToBackingFile, BACKINGNAME + INDEXER));
            cpmbf.Start();

            Thread.Sleep(3000);
            ProcMon.convertPMLfileToCSV(pathToBackingFile, BACKINGNAME + (INDEXER - 1) + ".PML", "convertedFile" + (INDEXER - 1) + ".CSV");
            //Thread.Sleep(3000);

            bool hasCSVbeenWritten = false;
            Console.WriteLine("Path to CSV file: " + pathToBackingFile + "\\" + "convertedFile" +(INDEXER - 1)+ ".CSV");

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
                
                List<CSVfileHandler> parsedData = CSVfileHandler.CSVparser(pathToBackingFile + "\\" + "convertedFile" + (INDEXER - 1) + ".CSV");
            
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

            try
            {
                /*
                Console.WriteLine("Process: " + Process.GetProcessById(pID.Last()).ProcessName + " is killed due to suspicious behaviour");
                killedProcesses.Add(Process.GetProcessById(pID.Last()).ProcessName);
                killProcess(pID.Last());*/
                if (!killedFirstProcess)
                {
                    firstKilledProcessTime = DateTime.Now;
                    killedFirstProcess = true;
                }
                
            }
            catch (Exception)
            {
                Console.WriteLine("Killing of  -- FAILED.");
            }
          
            //Console.WriteLine("Do you wish to kill? ");
            //string killInput = Console.ReadLine();
            



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
