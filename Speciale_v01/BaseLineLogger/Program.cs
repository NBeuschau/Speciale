using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaseLineLogger
{
    class Program
    {
        static string PATH = @"C:\Users\Quick";
        static void Main(string[] args)
        {
            baseLineTester();
        }

        public static void baseLineTester()
        {

            Thread.Sleep(5000);

            BaseLineLogger.getQuickHost();

            BaseLineLogger.LogWriter(PATH);
            BaseLineLogger.postQuickPosted();
        }
    }
}
