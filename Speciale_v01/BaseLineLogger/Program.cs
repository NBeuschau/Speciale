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
        //static string PATH = @"C:\Users\viruseater1";
        static string PATH = @"C:\Users\Baseline";
        static void Main(string[] args)
        {
            baseLineTester();
        }

        public static void baseLineTester()
        {

            /*
            BaseLineLogger.getBaseRansomware();
            Console.WriteLine(BaseLineLogger.getNAMEONTEST());
            BaseLineLogger.LogWriter(PATH);
            BaseLineLogger.postBasePosted();

            BaseLineLogger.postBaseTested();
            */

            BaseLineLogger.getBaseRansomware();
            Console.WriteLine(BaseLineLogger.getNAMEONTEST());
            BaseLineLogger.postBaseFetched();
            BaseLineLogger.test();
            BaseLineLogger.postBaseTested();

        }
    }
}
