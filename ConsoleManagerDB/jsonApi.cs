using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace ConsoleManagerDB
{
    public class WeatherForecast
    {
        public string InternetProtocol { get; set; }
        public string Date { get; set; }
        public string Method { get; set; }

    }



    public class Programs
    {
        public  void Write()
        {


            DataBase dataBase = new DataBase();
            string[] log = dataBase.GetLogs();

            var weatherForecast = new WeatherForecast
            {
                InternetProtocol = log[0],
                Date = log[1],
                Method = log[2],
            };
            string fileName = "select.json";
            string jsonString = JsonSerializer.Serialize(weatherForecast);
            File.WriteAllText(fileName, jsonString);

            StreamWriter sw = new StreamWriter("C:\\ConsoleManagerDB-master\\ConsoleManagerDB\\json\\select.json");
            sw.WriteLine(File.ReadAllText(fileName));
            Console.WriteLine("Записано в файл json/select.json");
            sw.Close();

            Console.WriteLine(File.ReadAllText(fileName));
        }
    }
}
