using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileChangeTypePOC
{
    class FilemonEventHandler
    {
        internal static void changeOccured(FileSystemEventArgs e)
        {
            //If this includes fileendings then react. Not sure it does though
            throw new NotImplementedException();
        }

        internal static void creationOccured(FileSystemEventArgs e)
        {
            //Find original file
            //Have all files in paths stored in a hashlist
            //When a creation has occured, see if all is still present, does missing remind of the created?
            throw new NotImplementedException();
        }

        internal static void deletionOccured(FileSystemEventArgs e)
        {
            //No reaction should be made here
            throw new NotImplementedException();
        }
    }
}
