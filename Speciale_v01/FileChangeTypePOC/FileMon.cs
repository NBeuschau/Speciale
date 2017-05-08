using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Collections;

namespace FileChangeTypePOC
{
    class FileMon
    {
        public static void CreateFileWatcher(string path)
        {
            //FileSystemWatcher can monitor changes in files
            FileSystemWatcher watcher = new FileSystemWatcher();

            //The given path dictates what directory the watcher will monitor
            watcher.Path = path;

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
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            //Cancel out appdata
            Console.WriteLine(e.FullPath + " is " + e.ChangeType);

            if (e.ChangeType.ToString().Equals("Changed"))
            {
                FilemonEventHandler.changeOccured(e);
            }
            else if (e.ChangeType.ToString().Equals("Created"))
            {
                FilemonEventHandler.creationOccured(e);
            }
            else if (e.ChangeType.ToString().Equals("Deleted"))
            {
                FilemonEventHandler.deletionOccured(e);
            }
        }


        //Event handeler if an object is renamed
        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            Console.WriteLine(e.OldFullPath + " is renamed to " + e.FullPath);
        }
    }
}
