using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLineLogger
{
    class Program
    {
        static string PATH = @"C:\Speciale\Test\";
        static void Main(string[] args)
        {
            baseLineTester();
        }

        public static void baseLineTester()
        {
            BaseLineLogger.LogWriter(PATH);
        }
    }
}
