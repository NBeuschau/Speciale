using HoneyPotFilemon.PocLogger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HoneyPotPOC.PocLogger
{
    class Logger
    {
        private static int INTERVALFORLOOP = 500;
        private static int MINUTESOFLOGGING = 25;
        private static string NAMEONTEST = "test";
        private static Boolean MONITORSTATUS = true;
        private static Boolean HASFETCHED = false;
        private static PerformanceCounter cpuUsageCounter;
        private static PerformanceCounter ramUsageCounter;
        private static PerformanceCounter harddiskUsageCounter;
        private static PerformanceCounter threadCounter;
        private static PerformanceCounter handleCounter;

        private static int amountOfLoops = 0;
        private static List<string> removeKeyList = new List<string>();
        private static List<string> changedKeyList = new List<string>();
        private static List<string> inStartDictionary = new List<string>();
        private static List<string> inEndDictionary = new List<string>();
        private static Dictionary<string, string>.KeyCollection hashedFilesAtStartKeys = null;
        private static Dictionary<string, string>.KeyCollection hashedFilesAtEndKeys = null;
        private static Dictionary<DateTime, string> fileMonChanges = Filemon.getFilemonChanges();
        private static List<float> cpuList = new List<float>();
        private static List<float> ramList = new List<float>();
        private static List<float> harddiskList = new List<float>();
        private static List<float> threadList = new List<float>();
        private static List<float> handleList = new List<float>();
        static string path1 = @"C:\Users\PoC\Desktop";
        static string path2 = @"C:\Users\PoC\Documents";
        static string path3 = @"C:\Users\PoC\Downloads";
        static string path4 = @"C:\Users\PoC\Videos";
        static string pathFileWatch = @"C:\Users\PoC";


        //Give the correct path for the hashed filesystem.
        //This includes giving the hasher the same path as the logger.
        static string hashedFilePath = @"C:\Software\";

        //static string path1 = @"C:\Users\viruseater1\Documents";
        //static string path2 = @"C:\Users\viruseater1\Desktop";
        //static string path3 = @"C:\Users\viruseater1\Downloads";
        //static string path4 = @"C:\Users\viruseater1\Videos";

        //Add the path to the ransomware downloader
        private static string ransomwareDownloaderPath = "";

        private static readonly HttpClient client = new HttpClient();

        public static Boolean LogWriter(string PATH)
        {

            cpuUsageCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramUsageCounter = new PerformanceCounter("Memory", "Available MBytes");
            harddiskUsageCounter = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");
            threadCounter = new PerformanceCounter("Process", "Thread Count", "_Total");
            handleCounter = new PerformanceCounter("Process", "Handle Count", "_Total");

            postPoCTaken();

            Dictionary<string, string> hashedFilesAtStart = new Dictionary<string, string>();

            //Get the hashed files from the txt file
            hashedFilesAtStart = testParseTXTfile(hashedFilePath);


            ProcMon.setIsHasherDone(true);
            amountOfLoops = 0;

            //After the hashed files has been read the ransomware is downloaded
            programExecuter.executeProgram(ransomwareDownloaderPath);


            var fw = new Thread(() => FileMon.createFileWatcher(pathFileWatch));
            fw.Start();

            var tmp = new Thread(() => Filemon.CreateFileWatcher(pathFileWatch));
            tmp.Start();

            //Find the start timestamp
            DateTime startTimeStamp = DateTime.Now;

            TimeSpan span = DateTime.Now.Subtract(startTimeStamp);

            //Loggs performance
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

            Filemon.setStopAddingToLog(true);
            fileMonChanges = Filemon.getFilemonChanges();

            Filemon.setWatcherToStop();
            FileMon.setWatcherToStop();
            ActionTaker.terminateProcmon();

            //Combines the hashed files from the four directories into one
            Dictionary<string, string> hashedFilesAtEnd = new Dictionary<string, string>();
            Dictionary<string, string> hashedFilesAtEndtemp1 = new Dictionary<string, string>();
            Dictionary<string, string> hashedFilesAtEndtemp2 = new Dictionary<string, string>();
            Dictionary<string, string> hashedFilesAtEndtemp3 = new Dictionary<string, string>();
            Dictionary<string, string> hashedFilesAtEndtemp4 = new Dictionary<string, string>();
            Hasher tempEndHasher1 = new Hasher();
            hashedFilesAtEndtemp1 = tempEndHasher1.fileHasher(path1);

            Hasher tempEndHasher2 = new Hasher();
            hashedFilesAtEndtemp2 = tempEndHasher2.fileHasher(path2);

            Hasher tempEndHasher3 = new Hasher();
            hashedFilesAtEndtemp3 = tempEndHasher3.fileHasher(path3);

            Hasher tempEndHasher4 = new Hasher();
            hashedFilesAtEndtemp4 = tempEndHasher4.fileHasher(path4);


            hashedFilesAtEndtemp1.ToList().ForEach(x => hashedFilesAtEnd.Add(x.Key, x.Value));
            hashedFilesAtEndtemp2.ToList().ForEach(x => hashedFilesAtEnd.Add(x.Key, x.Value));
            hashedFilesAtEndtemp3.ToList().ForEach(x => hashedFilesAtEnd.Add(x.Key, x.Value));
            hashedFilesAtEndtemp4.ToList().ForEach(x => hashedFilesAtEnd.Add(x.Key, x.Value));
            

            //Find the end timestamp
            DateTime endTimeStamp = DateTime.Now;

            //Figure out what has changed.
            removeKeyList = new List<string>();
            changedKeyList = new List<string>();
            inStartDictionary = new List<string>();
            inEndDictionary = new List<string>();
            foreach (var item in hashedFilesAtStart)
            {
                if (hashedFilesAtEnd.ContainsKey(item.Key))
                {
                    if (hashedFilesAtStart[item.Key].Equals(hashedFilesAtEnd[item.Key]))
                    {
                        removeKeyList.Add(item.Key);
                    }
                    else
                    {
                        changedKeyList.Add(item.Key);
                    }
                }
                else
                {
                    inStartDictionary.Add(item.Key);
                }
            }
            //Removing non changed duplicates
            for (int i = 0; i < removeKeyList.Count; i++)
            {
                hashedFilesAtStart.Remove(removeKeyList[i]);
                hashedFilesAtEnd.Remove(removeKeyList[i]);
            }
            for (int i = 0; i < changedKeyList.Count; i++)
            {
                hashedFilesAtStart.Remove(changedKeyList[i]);
                hashedFilesAtEnd.Remove(changedKeyList[i]);
            }
            //Finding files that has been created since start
            foreach (var item in hashedFilesAtEnd)
            {
                if (!hashedFilesAtStart.ContainsKey(item.Key))
                {
                    inEndDictionary.Add(item.Key);
                }
            }
            hashedFilesAtStartKeys = hashedFilesAtStart.Keys;
            hashedFilesAtEndKeys = hashedFilesAtEnd.Keys;
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

        public static async void postPoCPosted()
        {
            string cpuReturn = returnMonitorListAsString(cpuList);
            string ramReturn = returnMonitorListAsString(ramList);
            string harddiskReturn = returnMonitorListAsString(harddiskList);
            string threadReturn = returnMonitorListAsString(threadList);
            string handleReturn = returnMonitorListAsString(handleList);
            List<string> killedProcesses = ActionTaker.getKilledProcesses();

            string changedFilesReturn = "";
            string deletedFilesReturn = "";
            string newFilesReturn = "";
            string filemonChangesReturn = "";
            string killedProcessesReturn = "";


            for (int i = 0; i < changedKeyList.Count - 1; i++)
            {
                changedFilesReturn += changedKeyList[i];
                changedFilesReturn += "?";
            }
            foreach (string s in hashedFilesAtStartKeys)
            {
                deletedFilesReturn += s;
                deletedFilesReturn += "?";
            }
            foreach (string s in hashedFilesAtEndKeys)
            {
                newFilesReturn += s;
                newFilesReturn += "?";
            }
            foreach (var item in fileMonChanges)
            {
                filemonChangesReturn += item.Value + ":" + item.Key.ToString("dd/MM/yyyy HH:mm:ss.fff");
                filemonChangesReturn += "?";
            }
            foreach (string s in killedProcesses)
            {
                killedProcessesReturn += s;
                killedProcessesReturn += "?";
            }

            var options = new
            {
                RansomwareName = NAMEONTEST,
                MonitorStatus = "1",
                MonitorCount = amountOfLoops.ToString(),
                CountChangedFiles = changedKeyList.Count().ToString(),
                CountDeletedFiles = hashedFilesAtStartKeys.Count().ToString(),
                CountNewFiles = hashedFilesAtEndKeys.Count().ToString(),
                CountFilemonObservations = fileMonChanges.Count().ToString(),
                CPU = cpuReturn,
                RAM = ramReturn,
                HDD = harddiskReturn,
                ThreadCount = threadReturn,
                HandleCount = handleReturn,
                ListChangedFiles = changedFilesReturn,
                ListDeletedFiles = deletedFilesReturn,
                ListNewFiles = newFilesReturn,
                ListFilemonObservations = filemonChangesReturn,
                NameOfShutdownRansomware = killedProcessesReturn,
                Detected = FileMon.getFirstDetected().ToString("dd/MM/yyyy HH:mm:ss.fff"),
                Shutdown = ActionTaker.getFirstKilledTime().ToString("dd/MM/yyyy HH:mm:ss.fff")
            };


            var stringPayload = JsonConvert.SerializeObject(options);
            var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            var response = client.PostAsync("http://192.168.8.102/v1/index.php/posthp1posted", content).Result;
            var result = await response.Content.ReadAsByteArrayAsync();
        }

        public static void getPoCRansomware()
        {
            var responseString = client.GetStringAsync("http://192.168.8.102/v1/index.php/gethp1ransomware").Result;

            NAMEONTEST = findNAMEONTEST(responseString);
            Console.WriteLine(NAMEONTEST);
        }

        public static async void postPoCFetched()
        {
            var values = new Dictionary<string, string>
            {
                {"RansomwareName",  NAMEONTEST}
            };

            var content = new FormUrlEncodedContent(values);

            var response = client.PostAsync("http://192.168.8.102/v1/index.php/posthp1fetched", content).Result;

            HASFETCHED = true;

            var responseString = await response.Content.ReadAsByteArrayAsync();
        }


        private static string findNAMEONTEST(string responsestring)
        {
            int i = 0;
            int j = 0;
            foreach (char c in responsestring)
            {
                if (i == 5)
                {
                    return responsestring.Substring(j, responsestring.Length - j - 4);
                }
                if (c.Equals('"'))
                {
                    i++;
                }
                j++;
            }

            return "Could Not Find";
        }

        private static string returnMonitorListAsString(List<float> convertedList)
        {
            string temp = "";
            for (int i = 0; i < amountOfLoops; i++)
            {
                temp += convertedList[i].ToString();
                temp += "?";
            }
            return temp;
        }

        public static async void postPoCTested()
        {
            var values = new Dictionary<string, string>
            {
                {"RansomwareName",  NAMEONTEST}
            };

            var content = new FormUrlEncodedContent(values);

            var response = client.PostAsync("http://192.168.8.102/v1/index.php/posthp1tested", content).Result;

            var responseString = await response.Content.ReadAsByteArrayAsync();
        }

        public static async void postPoCTaken()
        {
            var values = new Dictionary<string, string>
            {
                {"RansomwareName",  NAMEONTEST}
            };

            var content = new FormUrlEncodedContent(values);

            var response = client.PostAsync("http://192.168.8.102/v1/index.php/posthp1taken", content).Result;

            var responseString = await response.Content.ReadAsByteArrayAsync();
        }


        public static string getNAMEONTEST()
        {
            return NAMEONTEST;
        }

        public static void setRansomwareDownloaderPath(string s)
        {
            ransomwareDownloaderPath = s;
        }

        public static Boolean getHasFetched()
        {
            return HASFETCHED;
        }

        public static Dictionary<string, string> testParseTXTfile(string hashedFilePath)
        {
            string line;
            string[] pairs = new string[2];
            Dictionary<string, string> hashedFilesReturn = new Dictionary<string, string>();
            System.IO.StreamReader file =
                new System.IO.StreamReader(hashedFilePath + "\\HashedFilesLog.txt");
            while ((line = file.ReadLine()) != null)
            {
                pairs = line.Split('?');
                hashedFilesReturn.Add(pairs[0], pairs[1]);
            }
            file.Close();

            return hashedFilesReturn;
        }
    }
}
