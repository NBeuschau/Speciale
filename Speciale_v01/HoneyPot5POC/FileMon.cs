using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Collections;

namespace HoneyPot5POC
{
    class FileMon
    {

        static int MONITORTIMEOUT = 60;
        static int thresholdNum = 1;
        public static int i = 0;
        public static int temp = 0;
        public static Dictionary<string, DateTime> eventNameAndTime = new Dictionary<string, DateTime>();
        private static Boolean hasMadeFirstDetection = false;
        private static DateTime firstDetectionTime = new DateTime();
        private static List<DateTime> threshold = new List<DateTime>();
        static Boolean stopLogging = false;
        private static FileSystemWatcher watcher = new FileSystemWatcher();

        public static void createFileWatcher(string path)
        {
            //FileSystemWatcher can monitor changes in files

            //The given path dictates what directory the watcher will monitor
            watcher.Path = path;

            //The NotifyFilters determine what the monitors triggers upon. 
            //It can also be a change in size.
            watcher.NotifyFilter = NotifyFilters.Size | NotifyFilters.LastWrite | NotifyFilters.FileName;

            //The filter gives the watcher a specific filename to look for
            // "*honeypot.*" monitors every file with honeypot in the ending, and every format.
            watcher.Filter = "*honeypotbait*";

            //This tells the watcher when to react on different changes
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);

            watcher.EnableRaisingEvents = true;
            //IncludeSubdirectories does such that not only the directory given is monitored
            //but also every single subdirectory of the given directory
            watcher.IncludeSubdirectories = true;
        }


        //Event handeler if an object is changed
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("File: " + e.FullPath + " has been " + e.ChangeType);
            threshold.Add(DateTime.Now);
            List<DateTime> temp = new List<DateTime>();
            DateTime now = DateTime.Now;
            foreach (DateTime t in threshold)
            {
                if (60 < (now.Subtract(t).Seconds))
                {
                    temp.Add(t);
                }
            }

            foreach (DateTime t in temp)
            {
                threshold.Remove(t);
            }


            if (threshold.Count > thresholdNum)
            {
                Console.WriteLine("Threshold reached. It's killing time");
                if (!hasMadeFirstDetection)
                {
                    firstDetectionTime = DateTime.Now;
                    hasMadeFirstDetection = true;
                }
                if (eventNameAndTime.ContainsKey(e.FullPath))
                {
                    Console.WriteLine("File: " + e.FullPath + " has been " + e.ChangeType);
                    if (MONITORTIMEOUT < (DateTime.Now.Subtract((DateTime)eventNameAndTime[e.FullPath])).TotalSeconds)
                    {
                        Console.WriteLine("Stopping the process fucking with MY honeypot!");
                        //Report it has been changed
                        eventNameAndTime[e.FullPath] = DateTime.Now;
                        ActionTaker.honeypotChange(e.FullPath);
                    }
                }
                else
                {
                    Console.WriteLine("File: " + e.FullPath + " has been " + e.ChangeType);
                    eventNameAndTime.Add(e.FullPath, DateTime.Now);
                    //Report it has been changed
                    ActionTaker.honeypotChange(e.FullPath);
                }
            }
        }

        //Event handeler if an object is renamed
        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            Console.WriteLine("Flie: {0} renamed to {1}", e.OldFullPath, e.FullPath);
        }

        public static DateTime getFirstDetected()
        {
            return firstDetectionTime;
        }

        public static void setWatcherToStop()
        {
            watcher.EnableRaisingEvents = false;
        }
    }
}
