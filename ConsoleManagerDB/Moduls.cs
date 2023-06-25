using Microsoft.IdentityModel.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleManagerDB
{
    public class LogAggregator
    {
        public class Logs
        {
            public string IP { get; set; }
            public DateTime DateTime { get; set; }
            public string Method { get; set; }
            public string Url { get; set; }
        }
        private string logFilePath;

        public LogAggregator(string filePath)
        {
            logFilePath = filePath;
        }

        public List<Logs> ReadLogs()
        {
            var logEntries = new List<Logs>();
           
            using (StreamReader reader = new StreamReader("C:\\ConsoleManagerDB-master\\ConsoleManagerDB\\logs\\access.log"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var entry = ParseLogLine(line);
                    if (entry != null)
                    {
                        logEntries.Add(entry);
                    }
                }
            }

            return logEntries;

        }
        
        private Logs ParseLogLine(string line)
        {
            var pattern = @"^([\d.]+) - - \[(.*?)\] ""(\w+) (.*?) HTTP/\d\.\d"" \d+ \d+$";
            var match = Regex.Match(line, pattern);

            if (match.Success)
            {
                var entry = new Logs
                {
                    IP = match.Groups[1].Value,
                    DateTime = DateTime.ParseExact(match.Groups[2].Value, "dd/MMM/yyyy:HH:mm:ss zzz", CultureInfo.InvariantCulture),
                    Method = match.Groups[3].Value,
                    Url = match.Groups[4].Value
                };

                return entry;
            }

            return null;
        }

    }
}