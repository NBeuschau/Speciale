using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Collections;

namespace QuickTestLoggerImproved
{
    class QTFileMon
    {

        Dictionary<DateTime, string> fileMonChanges = new Dictionary<DateTime, string>();
        public int i = 0;
        public int temp = 0;
        public static Hashtable eventTimeLog = new Hashtable();
        public void CreateFileWatcher(string PATH)
        {
            //FileSystemWatcher can monitor changes in files
            FileSystemWatcher watcher = new FileSystemWatcher();

            //The given path dictates what directory the watcher will monitor
            watcher.Path = PATH;

            //The NotifyFilters determine what the monitors triggers upon. 
            //It can also be a change in size.
            watcher.NotifyFilter = NotifyFilters.Size | NotifyFilters.LastWrite | NotifyFilters.FileName;

            //The filter gives the watcher a specific filename to look for
            // "*honeypot.*" monitors every file with honeypot in the ending, and every format.
            watcher.Filter = "*";

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
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            if (!e.FullPath.Contains("Data"))
            {
                if (!fileMonChanges.ContainsKey(DateTime.Now))
                {
                    Console.WriteLine("Changed " + e.FullPath);
                    fileMonChanges.Add(DateTime.Now, e.FullPath);
                }
            }
        }


        //Event handeler if an object is renamed
        private void OnRenamed(object source, RenamedEventArgs e)
        {
            if (!fileMonChanges.ContainsKey(DateTime.Now))
            {
                Console.WriteLine("Renamed " + e.FullPath);
                fileMonChanges.Add(DateTime.Now, e.FullPath);
            }
        }

        public Dictionary<DateTime, string> getFilemonChanges()
        {
            return fileMonChanges;
        }
    }
}
