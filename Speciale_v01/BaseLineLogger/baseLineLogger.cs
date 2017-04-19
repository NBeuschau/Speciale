﻿using Newtonsoft.Json;
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

namespace BaseLineLogger
{
    class BaseLineLogger
    {
        private static int INTERVALFORLOOP = 500;
        private static int MINUTESOFLOGGING = 25;
        private static string NAMEONTEST = "test";
        private static Boolean MONITORSTATUS = true;
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
        private static Dictionary<DateTime, string> fileMonChanges = FileMon.getFilemonChanges();
        private static List<float> cpuList = new List<float>();
        private static List<float> ramList = new List<float>();
        private static List<float> harddiskList = new List<float>();
        private static List<float> threadList = new List<float>();
        private static List<float> handleList = new List<float>();
        static string path1 = @"C:\Users\Baseline\Desktop";
        static string path2 = @"C:\Users\Baseline\Documents";
        static string path3 = @"C:\Users\Baseline\Downloads";
        static string path4 = @"C:\Users\Baseline\Videos";
        static string pathFileWatch = @"C:\Users\Baseline";

        //static string path1 = @"C:\Users\viruseater1\Documents";
        //static string path2 = @"C:\Users\viruseater1\Desktop";
        //static string path3 = @"C:\Users\viruseater1\Downloads";
        //static string path4 = @"C:\Users\viruseater1\Videos";

        private static readonly HttpClient client = new HttpClient();

        public static Boolean LogWriter(string PATH)
        {

            cpuUsageCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramUsageCounter = new PerformanceCounter("Memory", "Available MBytes");
            harddiskUsageCounter = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");
            threadCounter = new PerformanceCounter("Process", "Thread Count", "_Total");
            handleCounter = new PerformanceCounter("Process", "Handle Count", "_Total");

            //Find the start timestamp
            DateTime startTimeStamp = DateTime.Now;
            var fw = new Thread(() => FileMon.CreateFileWatcher(pathFileWatch));
            fw.Start();
            Dictionary<string, string> hashedFilesAtStart = new Dictionary<string, string>();
            Dictionary<string, string> hashedFilesAtStarttemp1 = new Dictionary<string, string>();
            Dictionary<string, string> hashedFilesAtStarttemp2 = new Dictionary<string, string>();
            Dictionary<string, string> hashedFilesAtStarttemp3 = new Dictionary<string, string>();
            Dictionary<string, string> hashedFilesAtStarttemp4 = new Dictionary<string, string>();
            Hasher tempStartHasher1 = new Hasher();
            hashedFilesAtStarttemp1 = tempStartHasher1.fileHasher(path1);

            Hasher tempStartHasher2 = new Hasher();
            hashedFilesAtStarttemp2 = tempStartHasher2.fileHasher(path2);

            Hasher tempStartHasher3 = new Hasher();
            hashedFilesAtStarttemp3 = tempStartHasher3.fileHasher(path3);

            Hasher tempStartHasher4 = new Hasher();
            hashedFilesAtStarttemp4 = tempStartHasher4.fileHasher(path4);


            hashedFilesAtStarttemp1.ToList().ForEach(x => hashedFilesAtStart.Add(x.Key, x.Value));
            hashedFilesAtStarttemp2.ToList().ForEach(x => hashedFilesAtStart.Add(x.Key, x.Value));
            hashedFilesAtStarttemp3.ToList().ForEach(x => hashedFilesAtStart.Add(x.Key, x.Value));
            hashedFilesAtStarttemp4.ToList().ForEach(x => hashedFilesAtStart.Add(x.Key, x.Value));


            amountOfLoops = 0;

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


            //Take a hash of the files at the end



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
            FileMon.setStopAddingToLog(true);
            fileMonChanges = FileMon.getFilemonChanges();
            /*
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
                    string cpuReturn = returnMonitorListAsString(cpuList);
                    string ramReturn = returnMonitorListAsString(ramList);
                    string harddiskReturn = returnMonitorListAsString(harddiskList);
                    string threadReturn = returnMonitorListAsString(threadList);
                    string handleReturn = returnMonitorListAsString(handleList);

                    string changedFilesReturn = "";
                    string deletedFilesReturn = "";
                    string newFilesReturn = "";
                    string filemonChangesReturn = "";

                    for (int i = 0; i < changedKeyList.Count; i++)
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
                        filemonChangesReturn += item.Value + "<>" + item.Key.ToString("dd/MM/yyyy HH:mm:ss.fff");
                        filemonChangesReturn += "?";
                    }

                    sw.WriteLine(cpuReturn);
                    sw.WriteLine(ramReturn);
                    sw.WriteLine(harddiskReturn);
                    sw.WriteLine(threadReturn);
                    sw.WriteLine(handleReturn);
                    sw.WriteLine(changedFilesReturn);
                    sw.WriteLine(deletedFilesReturn);
                    sw.WriteLine(newFilesReturn);
                    sw.WriteLine(filemonChangesReturn);

                }
            }*/
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

        public static async void postBasePosted()
        {
            string cpuReturn = returnMonitorListAsString(cpuList);
            string ramReturn = returnMonitorListAsString(ramList);
            string harddiskReturn = returnMonitorListAsString(harddiskList);
            string threadReturn = returnMonitorListAsString(threadList);
            string handleReturn = returnMonitorListAsString(handleList);

            string changedFilesReturn = "";
            string deletedFilesReturn = "";
            string newFilesReturn = "";
            string filemonChangesReturn = "";

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

            var options = new
            {
                RansomwareName = NAMEONTEST,
                MonitorStatus = "1" ,
                MonitorCount = amountOfLoops.ToString() ,
                CountChangedFiles = changedKeyList.Count().ToString() ,
                CountDeletedFiles = hashedFilesAtStartKeys.Count().ToString() ,
                CountNewFiles = hashedFilesAtEndKeys.Count().ToString() ,
                CountFilemonObservations = fileMonChanges.Count().ToString() ,
                CPU = cpuReturn,
                RAM = ramReturn,
                HDD = harddiskReturn,
                ThreadCount = threadReturn,
                HandleCount = handleReturn,
                ListChangedFiles = changedFilesReturn,
                ListDeletedFiles = deletedFilesReturn,
                ListNewFiles = newFilesReturn,
                ListFilemonObservations = filemonChangesReturn
            };


            var stringPayload = JsonConvert.SerializeObject(options);
            var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("http://192.168.8.102/v1/index.php/postbaseposted", content);
            var result = await response.Content.ReadAsByteArrayAsync();

            FileMon.setStopAddingToLog(false);
        }

        public static void getBaseRansomware()
        {
            var responseString = client.GetStringAsync("http://192.168.8.102/v1/index.php/getbaseransomware").Result;

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

        public static async void postBaseTested()
        {
            var values = new Dictionary<string, string>
            {
                {"RansomwareName",  NAMEONTEST}
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("http://192.168.8.102/v1/index.php/postbasetested", content);

            var responseString = await response.Content.ReadAsByteArrayAsync();
        }

        public static string getNAMEONTEST()
        {
            return NAMEONTEST;
        }











        public static async void test()
        {
            var values = new Dictionary<string, string>
            {
                {"RansomwareName", NAMEONTEST },
                {"MonitorStatus", "1" },
                {"MonitorCount", "test" },
                {"CountChangedFiles", "test" },
                {"CountDeletedFiles", "test" },
                {"CountNewFiles", "test" },
                {"CountFilemonObservations", "test" },
                {"CPU", "test"},
                {"RAM", "test"},
                {"HDD", "test"},
                {"ThreadCount", "test"},
                {"HandleCount", "test"},
                {"ListChangedFiles", "test"},
                {"ListDeletedFiles", "test" },
                {"ListNewFiles", "test"},
                {"ListFilemonObservations","Prut" }
            };

            using (var client = new HttpClient())
            {
                /*
                var options = new
                {
                    "RansomwareName" = NAMEONTEST ,
                "MonitorStatus" = "1" ,
                {"MonitorCount", "test" },
                {"CountChangedFiles", "test" },
                {"CountDeletedFiles", "test" },
                {"CountNewFiles", "test" },
                {"CountFilemonObservations", "test" },
                {"CPU", "test"},
                {"RAM", "test"},
                {"HDD", "test"},
                {"ThreadCount", "test"},
                {"HandleCount", "test"},
                {"ListChangedFiles", "test"},
                {"ListDeletedFiles", "test"},
                {"ListNewFiles", "test"},
                {"ListFilemonObservations", "te"}
                ;}
                */
                var options1 = new
                {
                    value = "test",
                    apikey = ConfigurationManager.AppSettings["pdf:key"],
                    MarginLeft = "10",
                    MarginRight = "10"
                };

                var options = new
                {
                    RansomwareName = NAMEONTEST,
                    MonitorStatus = "1",
                    MonitorCount = "test",
                    CountChangedFiles = "test",
                    CountDeletedFiles = "test",
                    CountNewFiles = "test",
                    CountFilemonObservations = "test",
                    CPU = "test",
                    RAM = "test",
                    HDD = "test",
                    ThreadCount = "test",
                    HandleCount = "test",
                    ListChangedFiles = "test",
                    ListDeletedFiles = "test",
                    ListNewFiles = "test",
                    ListFilemonObservations = "test"
                };
                
                var stringPayload = JsonConvert.SerializeObject(options);
                var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("http://192.168.8.102/v1/index.php/postbaseposted", content);
                var result = await response.Content.ReadAsByteArrayAsync();
            }

                //NYT
                /*
                using (var client = new HttpClient())
                {
                    // Build the conversion options
                    var options = new Dictionary<string, string>
                    {
                        { "value", html },
                        { "apikey", ConfigurationManager.AppSettings["pdf:key"] },
                        { "MarginLeft", "10" },
                        { "MarginRight", "10" }
                    };

                    // THIS LINE RAISES THE EXCEPTION
                    var content = new FormUrlEncodedContent(options);

                    var response = await client.PostAsync("https://api.html2pdfrocket.com/pdf", content);
                    var result = await response.Content.ReadAsByteArrayAsync();
                    return result;
                }

                using (var client = new HttpClient())
                {
                    // Build the conversion options
                    var options = new
                    {
                        value = html,
                        apikey = ConfigurationManager.AppSettings["pdf:key"],
                        MarginLeft = "10",
                        MarginRight = "10"
                    };

                    // Serialize our concrete class into a JSON String
                    var content = JsonConvert.SerializeObject(options);
                    var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("https://api.html2pdfrocket.com/pdf", content);
                    var result = await response.Content.ReadAsByteArrayAsync();
                    return result;
                }

                var values = new List<KeyValuePair<string, string>>();
                values.Add(new KeyValuePair<string, string>("data", XMLBody));
                var content = new FormUrlEncodedContent(values);
                HttpResponseMessage sResponse = await sClient.PostAsync(action.URL, content).ConfigureAwait(false);


                StringContent content = new StringContent("data=" + HttpUtility.UrlEncode(action.Body), Encoding.UTF8, "application/x-www-form-urlencoded");
                HttpResponseMessage sResponse = await sClient.PostAsync(action.URL, content).ConfigureAwait(false);
                */
                //NYT

                //var content = new FormUrlEncodedContent(values);

        }

    }
}
