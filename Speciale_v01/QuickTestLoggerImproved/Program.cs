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
        static string path1 = @"C:\Speciale\Test\Test1";
        static string path2 = @"C:\Speciale\Test\Test2";
        static string path3 = @"C:\Speciale\Test\Test3";
        static string path4 = @"C:\Speciale\Test\Test4";
        static void Main(string[] args)
        {
            quickTestStarter();
        }

        private static void quickTestStarter()
        {

            Thread.Sleep(60000);

            QTLogger.getQuickHost();

            QTLogger.LogWriter(path1, path2, path3, path4);

            QTLogger.postQuickPosted();
        }
    }
}
