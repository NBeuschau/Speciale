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

namespace BaseLineLogger
{
    class BaseLineLogger
    {
        //Interval for how often it will monitor CPU, RAM, harddisk, Thread count, Handle count
        private static int INTERVALFORLOOP = 500;
        //How long it will do a log of what is happening on the computer before hashing and sending
        private static int MINUTESOFLOGGING = 25;
        
        //Global nessesary variables
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
        private static readonly HttpClient client = new HttpClient();

        //Path names for the folders the filemonitor is monitoring
        static string path1 = @"C:\Users\Baseline\Desktop";
        static string path2 = @"C:\Users\Baseline\Documents";
        static string path3 = @"C:\Users\Baseline\Downloads";
        static string path4 = @"C:\Users\Baseline\Videos";
        static string pathFileWatch = @"C:\Users\Baseline";

        //Give the correct path for the hashed filesystem.
        //This includes giving the hasher the same path as the logger.
        static string hashedFilePath = @"C:\Software\";


        //Add the path to the ransomware downloader
        private static string ransomwareDownloaderPath = "";

        //static string path1 = @"C:\Users\viruseater1\Documents";
        //static string path2 = @"C:\Users\viruseater1\Desktop";
        //static string path3 = @"C:\Users\viruseater1\Downloads";
        //static string path4 = @"C:\Users\viruseater1\Videos";


        public static Boolean LogWriter(string PATH)
        {
            //Sets what aspects are to be monitored
            cpuUsageCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramUsageCounter = new PerformanceCounter("Memory", "Available MBytes");
            harddiskUsageCounter = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");
            threadCounter = new PerformanceCounter("Process", "Thread Count", "_Total");
            handleCounter = new PerformanceCounter("Process", "Handle Count", "_Total");




            //Create the dictionaries for the hashed files for each path
            Dictionary<string, string> hashedFilesAtStart = new Dictionary<string, string>();

            hashedFilesAtStart = testParseTXTfile(hashedFilePath);

            programExecuter.executeProgram(ransomwareDownloaderPath);

            postBaseTaken();

            //Adds the hashed files to a single list
            //Starts the filewatcher
            var fw = new Thread(() => FileMon.CreateFileWatcher(pathFileWatch));
            fw.Start();

            //Find the start timestamp
            DateTime startTimeStamp = DateTime.Now;

            amountOfLoops = 0;

            //Loops in an interval given by MINUTESOFLOGGING
            TimeSpan span = DateTime.Now.Subtract(startTimeStamp);
            Console.WriteLine("Starting loop");
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
            Console.WriteLine("Ending loop \n starting process to post");

            //Stops the filemon log such that there aren't written to it during iteration
            FileMon.setStopAddingToLog(true);
            fileMonChanges = FileMon.getFilemonChanges();

            FileMon.setWatcherToStop();


            //Creates the dictionaries for the hashed files at the end
            Dictionary<string, string> hashedFilesAtEnd = new Dictionary<string, string>();
            Dictionary<string, string> hashedFilesAtEndtemp1 = new Dictionary<string, string>();
            Dictionary<string, string> hashedFilesAtEndtemp2 = new Dictionary<string, string>();
            Dictionary<string, string> hashedFilesAtEndtemp3 = new Dictionary<string, string>();
            Dictionary<string, string> hashedFilesAtEndtemp4 = new Dictionary<string, string>();

            //Hashes the files and adds them to the dictionaries
            Hasher tempEndHasher1 = new Hasher();
            hashedFilesAtEndtemp1 = tempEndHasher1.fileHasher(path1);

            Hasher tempEndHasher2 = new Hasher();
            hashedFilesAtEndtemp2 = tempEndHasher2.fileHasher(path2);

            Hasher tempEndHasher3 = new Hasher();
            hashedFilesAtEndtemp3 = tempEndHasher3.fileHasher(path3);

            Hasher tempEndHasher4 = new Hasher();
            hashedFilesAtEndtemp4 = tempEndHasher4.fileHasher(path4);

            //Adds all dictonaries to a single one.
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
                //If the hashed files at start and hashed files at end is of the same path
                if (hashedFilesAtEnd.ContainsKey(item.Key))
                {
                    //If they have the same hashvalue
                    if (hashedFilesAtStart[item.Key].Equals(hashedFilesAtEnd[item.Key]))
                    {
                        //Add them to a list where these elements have not been hit by ransomware
                        removeKeyList.Add(item.Key);
                    }
                    else
                    {
                        //If the path is the same but the hash is different then it has been changed
                        changedKeyList.Add(item.Key);
                    }
                }
                else
                {
                    //If it is in start but not in the end then it has been deleted
                    inStartDictionary.Add(item.Key);
                }
            }
            //Removing non changed duplicates
            for (int i = 0; i < removeKeyList.Count; i++)
            {
                hashedFilesAtStart.Remove(removeKeyList[i]);
                hashedFilesAtEnd.Remove(removeKeyList[i]);
            }
            //Removes changed files, they are still stored in the list changedKeyList
            for (int i = 0; i < changedKeyList.Count; i++)
            {
                hashedFilesAtStart.Remove(changedKeyList[i]);
                hashedFilesAtEnd.Remove(changedKeyList[i]);
            }
            //Finding files that has been created since start
            foreach (var item in hashedFilesAtEnd)
            {
                //If it is in the end but not the start then it has been created since
                if (!hashedFilesAtStart.ContainsKey(item.Key))
                {
                    inEndDictionary.Add(item.Key);
                }
            }
            hashedFilesAtStartKeys = hashedFilesAtStart.Keys;
            hashedFilesAtEndKeys = hashedFilesAtEnd.Keys;



            //Old part, can create a txt log of what is observed
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

        //Posts to the server the results of this baseline
        public static async void postBasePosted()
        {
            //Turns the monitored data into strings
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

            //Creates the element that is send with JSON
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

            //Converts this into a payload
            var stringPayload = JsonConvert.SerializeObject(options);
            var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            //Send to the server
            var response = client.PostAsync("http://192.168.8.102/v1/index.php/postbaseposted", content).Result;
            var result = await response.Content.ReadAsByteArrayAsync();

            //Start the filemon log again, this doesn't have a purpose
            FileMon.setStopAddingToLog(false);
        }

        //Get the name on the ransomware that is to be tested
        public static void getBaseRansomware()
        {
            var responseString = client.GetStringAsync("http://192.168.8.102/v1/index.php/getbaseransomware").Result;

            NAMEONTEST = findNAMEONTEST(responseString);
        }

        //Parse the string from the server into a simple name
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

        //Takes a monitor list and returns it as a string
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

        //Posts to the server that this ransomware has been tested on the baseline
        public static async void postBaseTested()
        {
            var values = new Dictionary<string, string>
            {
                {"RansomwareName",  NAMEONTEST}
            };

            var content = new FormUrlEncodedContent(values);

            var response = client.PostAsync("http://192.168.8.102/v1/index.php/postbasetested", content).Result;

            var responseString = await response.Content.ReadAsByteArrayAsync();
        }

        //Informs the server that the data has been downloaded, thus creating an empty 
        public static async void postBaseFetched()
        {
            var values = new Dictionary<string, string>
            {
                {"RansomwareName",  NAMEONTEST}
            };

            var content = new FormUrlEncodedContent(values);

            var response = client.PostAsync("http://192.168.8.102/v1/index.php/postbasefetched", content).Result;

            var responseString = await response.Content.ReadAsByteArrayAsync();
        }

        //Post to the server in the quicktester that the ransomware has been taken
        public static async void postBaseTaken()
        {
            var values = new Dictionary<string, string>
            {
                {"RansomwareName",  NAMEONTEST}
            };

            var content = new FormUrlEncodedContent(values);

            var response = client.PostAsync("http://192.168.8.102/v1/index.php/postbasetaken", content).Result;

            var responseString = await response.Content.ReadAsByteArrayAsync();
        }

        //Returns the name on the test
        public static string getNAMEONTEST()
        {
            return NAMEONTEST;
        }

        public static void setRansomwareDownloaderPath(string s)
        {
            ransomwareDownloaderPath = s;
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
