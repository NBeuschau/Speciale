using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Speciale_v01.TestEnvironmentLogger
{
    class Logger
    {
        private static int INTERVALFORLOOP = 20000;
        private static int MINUTESOFLOGGING = 1;
        private static string NAMEONTEST = "test";
        private static Boolean MONITORSTATUS = true;
        private static PerformanceCounter cpuUsageCounter;
        private static PerformanceCounter ramUsageCounter;
        private static PerformanceCounter harddiskUsageCounter;
        private static PerformanceCounter threadCounter;
        private static PerformanceCounter handleCounter;


        public static Boolean LogWriter(string path){

            cpuUsageCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramUsageCounter = new PerformanceCounter("Memory", "Available MBytes");
            harddiskUsageCounter = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");
            threadCounter = new PerformanceCounter("Process", "Thread Count", "_Total");
            handleCounter = new PerformanceCounter("Process", "Handle Count", "_Total");


            List<float> cpuList = new List<float>();
            List<float> ramList = new List<float>();
            List<float> harddiskList = new List<float>();
            List<float> threadList = new List<float>();
            List<float> handleList = new List<float>();
            Dictionary<string,string> hashedFilesAtStart = Hasher.fileHasher(path);
            //Find the name of the test

            //Check if the monitor is still active

            //Find the start timestamp
            DateTime startTimeStamp = DateTime.Now;

            int amountOfLoops = 0;
            TimeSpan span = DateTime.Now.Subtract(startTimeStamp);
            while (span.Minutes < MINUTESOFLOGGING)
            {
                amountOfLoops++;

                cpuList.Add(getCurrentCpuUsage());
                ramList.Add(getAvailableRAM());
                harddiskList.Add(getHarddiskUsage());
                threadList.Add(getThreadCount());
                handleList.Add(getHandleCount());

                Thread.Sleep(INTERVALFORLOOP);


                span = DateTime.Now.Subtract(startTimeStamp);
            }


            //Find ud af hvor mange krypterede filer der er 
            //Og hvilke
            //Og hvad deres sti er
            //Og få det hele tilbage i en liste således at det kan skrives til txt
            //Find ud af hvor mange er blevet opsnappet af filemonitor
            //Skriv det hele i en liste af hvad der bliver opdaget af filemonitor


            //Find the end timestamp
            DateTime endTimeStamp = DateTime.Now;

            //Take a hash of the files at the end
            Dictionary<string,string> hashedFilesAtEnd = Hasher.fileHasher(path);


            //Figure out what has changed.
            foreach (var item in hashedFilesAtStart)
            {
                //Hvis der er to ens keys i begge dictionaries og begge har samme value. Tilføj den key til ting der skal slettes.
                //Hvis der er en key i start som ikke er i slut ...
                //Hvis der er en key i slut som ikke er i start ...

            }


            string filePath = @path + "\\RansomwareLog1.txt";
            if (!File.Exists(filePath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine(NAMEONTEST);
                    sw.WriteLine(MONITORSTATUS);
                    sw.WriteLine(startTimeStamp.ToString());
                    sw.WriteLine(endTimeStamp.ToString());
                    sw.WriteLine(amountOfLoops);
                    sw.WriteLine(hashedFilesAtStart.Count());
                    for (int i = 0; i < amountOfLoops; i++)
                    {
                        sw.WriteLine(cpuList[i].ToString());
                    }
                    for (int i = 0; i < amountOfLoops; i++)
                    {
                        sw.WriteLine(ramList[i].ToString());
                    }
                    for (int i = 0; i < amountOfLoops; i++)
                    {
                        sw.WriteLine(harddiskList[i].ToString());
                    }
                    for (int i = 0; i < amountOfLoops; i++)
                    {
                        sw.WriteLine(threadList[i].ToString());
                    }
                    for (int i = 0; i < amountOfLoops; i++)
                    {
                        sw.WriteLine(handleList[i].ToString());
                    }
                    /*for (int i = 0; i < hashedFiles.Count(); i++)
                    {
                        sw.WriteLine(hashedFiles[i]);
                    }*/
                }
            }
            return true;
        }

        private static float getCurrentCpuUsage()
        {
            return cpuUsageCounter.NextValue();
        }

        private static float getAvailableRAM()
        {
            return ramUsageCounter.NextValue();
        }

        private static float getHarddiskUsage()
        {
            return harddiskUsageCounter.NextValue();
        }

        private static float getThreadCount()
        {
            return threadCounter.NextValue();
        }

        private static float getHandleCount()
        {
            return handleCounter.NextValue();
        }
    }
}
