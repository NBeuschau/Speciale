
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HoneyPotPOC
{
    class CSVfileHandler
    {
        public CSVfileHandler() { }
        public CSVfileHandler(string timeOfDay, string processName, int PID, string operation, string path, string result, string detail)
        {
            this.timeOfDay = timeOfDay;
            this.processName = processName;
            this.PID = PID;
            this.operation = operation;
            this.path = path;
            this.result = result;
            this.detail = detail;
        }

        //Parse the CSVfile to a class
        public static List<CSVfileHandler> CSVparser(string path)
        {
            List<CSVfileHandler> output = new List<CSVfileHandler>();

            StreamReader sr = new StreamReader(path);

            string line;
            string[] row = new string[7];
            line = sr.ReadLine();
            while ((line = sr.ReadLine()) != null)
            {
                row = line.Split(new string[] { "\",\"" }, StringSplitOptions.None);
                CSVfileHandler temp = new CSVfileHandler();
                for (int i = 0; i < row.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            temp.timeOfDay = row[i].Substring(1);
                            break;
                        case 1:
                            temp.processName = row[i];
                            break;
                        case 2:
                            int x = 0;
                            Int32.TryParse(row[i], out x);
                            temp.PID = x;
                            break;
                        case 3:
                            temp.operation = row[i];
                            break;
                        case 4:
                            temp.path = row[i];
                            break;
                        case 5:
                            temp.result = row[i];
                            break;
                        case 6:
                            temp.detail = row[i];
                            break;
                        default:
                            break;
                    }
                }
                output.Add(temp);
            }
            return output;
        }

        public string timeOfDay { get; set; }
        public string processName { get; set; }
        public int PID { get; set; }
        public string operation { get; set; }
        public string path { get; set; }
        public string result { get; set; }
        public string detail { get; set; }
    }
}
