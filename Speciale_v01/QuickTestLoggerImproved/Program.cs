using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuickTestLoggerImproved
{
    class Program
    {
        static string path1 = @"C:\Users\Quick\Desktop";
        static string path2 = @"C:\Users\Quick\Documents";
        static string path3 = @"C:\Users\Quick\Downloads";
        static string path4 = @"C:\Users\Quick\Videos";
        static void Main(string[] args)
        {
            quickTestStarter();
        }

        private static void quickTestStarter()
        {

            Thread.Sleep(21000);

            QTLogger.getQuickHost();

            QTLogger.LogWriter(path1, path2, path3, path4);

            QTLogger.postQuickPosted();

            Thread.Sleep(5000);
        }
    }
}
