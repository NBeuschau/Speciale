using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Speciale_v01
{
    class Watcher
    {
        public void CreateFileWatcher(string path)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();

            watcher.Path = path;

            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            watcher.Filter = "*test.*";

            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);

            watcher.EnableRaisingEvents = true;

        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
            //Console.WriteLine("Locking process is: {0}", IdentifyLocker.getPID(e.FullPath));
            IdentifyLocker.getPID(e.FullPath);
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            Console.WriteLine("Flie: {0} renamed to {1}", e.OldFullPath, e.FullPath);
            IdentifyLocker.getPID(e.FullPath);
        }

    }
}
