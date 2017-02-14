using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Speciale_v01
{
    class Procmon
    {
        public void createProcmonBatchFile(string path, string backingName)
        {
            string backPath = path + @"\" + backingName;

            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine(@"start C:\Speciale\Tools\procmon\procmon.exe /quiet /minimized /AcceptEula /backingfile C:\Speciale\Test\" + backingName + ".PML");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());


        }

        public void convertPMLfileToCSV(string path, string PMLfile, string CSVfile)
        {
            path = path + @"\";
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine(@"start C:\Speciale\Tools\procmon\procmon.exe /quiet /minimized /AcceptEula /saveas " + path + CSVfile + " /OpenLog " + path + PMLfile);
            Thread.Sleep(1000);
            int i = 0;
            long length = new System.IO.FileInfo(path + CSVfile).Length;
            long temp = 0;
            while (length != temp)
            {
                i++;
                temp = length;
                Thread.Sleep(10);
                length = new System.IO.FileInfo(path + CSVfile).Length;
            }
            Console.WriteLine("FÆRDIG! Efter " + i + " lykker");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());
        }
    }
}
