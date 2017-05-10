using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HoneyPot10POC
{
    class ProcMon
    {
        private static Process cmd = new Process();
        private static string procMonPath = "";
        private static Boolean isHasherDone = false;
        public static void createProcmonBackingFile(string path, string backingName)
        {
            while (!isHasherDone)
            {
                Thread.Sleep(500);
            }
            string backPath = path + @"\" + backingName;

            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine(@"start " + procMonPath + @" /quiet /minimized /backingfile " + path + "\\" + backingName + ".PML");
            Console.WriteLine("Path to procMon file: " + path + "\\" + backingName);
            cmd.StandardInput.Flush();
        }

        public static void procmonTerminator(string path, string backingName)
        {
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine(procMonPath + " /waitforidle");
            cmd.StandardInput.WriteLine(procMonPath + " /terminate");
            Console.WriteLine("Path to procMon file: " + path + "\\" + backingName + ".PML");
            bool isProcMonTerminated = false;

            while (isProcMonTerminated == false)
            {

                Process[] pname = Process.GetProcessesByName("Procmon64");
                if (pname.Length == 0)
                {
                    Console.WriteLine("Procmon is no longer running, continuing...");
                    isProcMonTerminated = true;
                }
                else {
                    Console.WriteLine("Procmon64 process is running!");
                }
                    //Console.WriteLine("Process found!");


              /*  try
                {
                //    fs = file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                  //  isProcMonTerminated = true;
                   // fs.Dispose();

                    using (Stream stream = new FileStream(path + "\\" + backingName + ".PML", FileMode.Open))
                    {
                        isProcMonTerminated = true;
                        stream.Dispose();
                    }
                }
                catch (IOException)
                {

                } */
                Thread.Sleep(50);
            }
            /*
            bool tmp = cmd.HasExited;
            Console.WriteLine("Has the process exited? : " + tmp);
            while (!cmd.HasExited) {
                tmp = cmd.HasExited;
                Console.WriteLine("Has the process exited? : " + tmp);
            };
            tmp = cmd.HasExited;
            Console.WriteLine("Has the process exited? : " + tmp);*/
        }

        public static void convertPMLfileToCSV(string path, string PMLfile, string CSVfile)
        {
            path = path + @"\";
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine(@"start " + procMonPath + " /quiet /minimized /AcceptEula /SaveApplyFilter /saveas " + path + CSVfile + " /OpenLog " + path + PMLfile);
            Thread.Sleep(5000);
            int i = 0;
            long length = 0;
            while (!File.Exists(path + CSVfile))
            {
                try
                {
                    length = new System.IO.FileInfo(path + CSVfile).Length;
                }
                catch (Exception)
                {
                }
                Thread.Sleep(50);
            }
            long temp = 0;
            while (length != temp)
            {
                i++;
                temp = length;
                Thread.Sleep(50);
                length = new System.IO.FileInfo(path + CSVfile).Length;
            }
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());
        }

        public static void setPathToProcMon(string path)
        {
            procMonPath = path;
        }

        public static void setIsHasherDone(Boolean b)
        {
            isHasherDone = b;
        }
    }
}
