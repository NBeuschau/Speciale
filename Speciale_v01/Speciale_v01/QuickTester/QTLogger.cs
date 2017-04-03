using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Speciale_v01.TestEnvironmentLogger
{
    class QTLogger
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

        private static string fileChangedOnHash = "0";
        private static string fileChangedOnWatcher = "0";
        private static readonly HttpClient client = new HttpClient();


        public static Boolean LogWriter(string PATH)
        {

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

            //Find the start timestamp
            DateTime startTimeStamp = DateTime.Now;
            var fw = new Thread(() => QTFileMon.CreateFileWatcher(PATH));
            fw.Start();

            Dictionary<string, string> hashedFilesAtStart = new Dictionary<string, string>();
            QTHasher tempHasher1 = new QTHasher();
            hashedFilesAtStart = tempHasher1.fileHasher(PATH);
            Dictionary<string, string> tempHashedFilesAtStart = new Dictionary<string, string>();


            //Find the name of the test
            int amountOfLoops = 0;


            //Take a hash of the files at the end
            QTHasher tempHasher2 = new QTHasher();
            Dictionary<string, string> hashedFilesAtEnd = new Dictionary<string, string>();
            Boolean activity = false;

            Dictionary<string, string>.KeyCollection hashedFilesAtStartKeys = null;
            Dictionary<string, string>.KeyCollection hashedFilesAtEndKeys = null;

            Dictionary<DateTime, string> fileMonChanges = null;

            DateTime endTimeStamp = DateTime.Now;
            List<string> removeKeyList = new List<string>();
            List<string> changedKeyList = new List<string>();
            List<string> inStartDictionary = new List<string>();
            List<string> inEndDictionary = new List<string>();


            while (!activity)
            {
                foreach (string key in hashedFilesAtStart.Keys)
                {
                    if (!tempHashedFilesAtStart.ContainsKey(key))
                    {
                        tempHashedFilesAtStart.Add(key, hashedFilesAtStart[key]);
                    }
                }
                Thread.Sleep(5000);

                hashedFilesAtEnd = new Dictionary<string, string>();
                hashedFilesAtEnd = tempHasher2.fileHasher(PATH);

                //Find the end timestamp
                endTimeStamp = DateTime.Now;

                //Figure out what has changed.
                removeKeyList = new List<string>();
                changedKeyList = new List<string>();
                inStartDictionary = new List<string>();
                inEndDictionary = new List<string>();
                foreach (var item in tempHashedFilesAtStart)
                {
                    if (hashedFilesAtEnd.ContainsKey(item.Key))
                    {
                        if (tempHashedFilesAtStart[item.Key].Equals(hashedFilesAtEnd[item.Key]))
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
                    tempHashedFilesAtStart.Remove(removeKeyList[i]);
                    hashedFilesAtEnd.Remove(removeKeyList[i]);
                }
                for (int i = 0; i < changedKeyList.Count; i++)
                {
                    tempHashedFilesAtStart.Remove(changedKeyList[i]);
                    hashedFilesAtEnd.Remove(changedKeyList[i]);
                }
                //Finding files that has been created since start
                foreach (var item in hashedFilesAtEnd)
                {
                    if (!tempHashedFilesAtStart.ContainsKey(item.Key))
                    {
                        inEndDictionary.Add(item.Key);
                    }
                }
                hashedFilesAtStartKeys = tempHashedFilesAtStart.Keys;
                hashedFilesAtEndKeys = hashedFilesAtEnd.Keys;

                fileMonChanges = QTFileMon.getFilemonChanges();
                if (hashedFilesAtEndKeys.Count != 0 || hashedFilesAtStartKeys.Count != 0)
                {
                    activity = true;
                    if (fileMonChanges.Count != 0)
                    {
                        fileChangedOnWatcher = "1";
                    }
                    if (hashedFilesAtEndKeys.Count != 0 || hashedFilesAtStartKeys.Count != 0)
                    {
                        fileChangedOnHash = "1";
                    }
                }
            }

            string filePath = PATH + "\\RansomwareLog.txt";
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
                    sw.WriteLine(changedKeyList.Count);
                    sw.WriteLine(hashedFilesAtStartKeys.Count);
                    sw.WriteLine(hashedFilesAtEndKeys.Count);
                    sw.WriteLine(fileMonChanges.Count);
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
                    for (int i = 0; i < changedKeyList.Count; i++)
                    {
                        sw.WriteLine(changedKeyList[i]);
                    }
                    foreach (string s in hashedFilesAtStartKeys)
                    {
                        sw.WriteLine(s);
                    }
                    foreach (string s in hashedFilesAtEndKeys)
                    {
                        sw.WriteLine(s);
                    }
                    foreach (var item in fileMonChanges)
                    {
                        sw.WriteLine(item.Value + ", " + item.Key.ToString("dd/MM/yyyy HH:mm:ss.fff"));
                    }
                }
            }
            postQuickFetched();
            return true;
        }

        private static async void postQuickPosted()
        {
            var values = new Dictionary<string, string>
            {
                {"FileChangedOnHash", fileChangedOnHash},
                {"FileChangedOnWatcher", fileChangedOnWatcher},
                {"Active", "Yes" },
                {"RansomwareName",  NAMEONTEST}
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("http://192.168.8.102/v1/index.php/postquickposted", content);

            var responseString = await response.Content.ReadAsByteArrayAsync();
        }

        public static async void postQuickFetched()
        {
            var values = new Dictionary<string, string>
            {
                {"RansomwareName",  NAMEONTEST}
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("http://192.168.8.102/v1/index.php/postquickfetched", content);

            var responseString = await response.Content.ReadAsByteArrayAsync();
        }

        public static void getQuickRansomware()
        {

            var responseString = client.GetStringAsync("http://192.168.8.102/v1/index.php/getquickransomware").Result;

            NAMEONTEST = findNAMEONTEST(responseString);

        }

        public static async void getQuickHost()
        {
            var responseString = await client.GetStringAsync("http://192.168.8.102/v1/index.php/getquickhost");

            NAMEONTEST = findNAMEONTEST(responseString);
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

            return "what?";
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

        public static string getNAMEONTEST()
        {
            return NAMEONTEST;
        }
    }
}
