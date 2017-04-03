using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuickTestLogger
{
    class Program
    {
        static string PATH = @"C:\Users\Quick";
        static void Main(string[] args)
        {
            quickTester();
        }

        public static void quickTester()
        {
            //Sleep before starting, such that the ransomware has been installed
            Thread.Sleep(60000);

            QTLogger.getQuickHost();

            QTLogger.LogWriter(PATH);

            QTLogger.postQuickPosted();
        }




    }
}
