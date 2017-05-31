using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector
{
    class FileObject
    {
        public string Path { get; set; }
        public string Size { get; set; }
        public string LastModified { get; set; }
        public string DateCreated { get; set; }
        public string LastAccessed { get; set; }
    }
}
