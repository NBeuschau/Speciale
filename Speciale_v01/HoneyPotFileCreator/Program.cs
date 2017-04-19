using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HoneyPotFileCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo d = new DirectoryInfo(@"D:\Honeypotfiles");
            FileInfo[] infos = d.GetFiles();
            string tempString = "";
            foreach (FileInfo f in infos)
            {

                tempString = f.Name.Substring(0, f.Name.Length-5) + "honeypot" + f.Name.Substring(f.Name.Length-5,5);
                File.Move(f.FullName, Path.Combine(f.Directory.ToString(), tempString));

            }
        }
    }
}
