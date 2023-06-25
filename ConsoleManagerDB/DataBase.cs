using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Net;
using System.Net.Sockets;
using Org.BouncyCastle.Asn1.Cmp;
using System.Xml.Linq;
using System.Data;
using System.Windows.Forms;
using Google.Protobuf.WellKnownTypes;

namespace ConsoleManagerDB
{
    
    public class DataBase
    {
        private MySqlConnection connection;
        private string Server;
        private string Database;
        private string UserName;
        private string Password;

        public DataBase()
        {
            Server = "sql7.freesqldatabase.com";
            Database = "sql7627553";
            UserName = "sql7627553";
            Password = "EcdmVq645g";
        }

        public bool OpenConection()
        {
            string conection = $"Server={Server}; DATABASE={Database};UID={UserName};PASSWORD={Password}";
            connection = new MySqlConnection(conection);
            int maxRetryCount = 3;
            int currentRetry = 0;
            while (currentRetry < maxRetryCount)
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (MySqlException ec)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Ошибка подключения к базе данных {ec.Message}");
                    currentRetry++;
                }
            }
            return false;
        }
        public void CloseConecton()
        {
            connection.Close();
        }

        public void InsertInformation(string internetProtocol, DateTime dateTime)
        {
            OpenConection();
            string quary = "INSERT INTO Information (Internet_Protocol, Data_Time) VALUES (@internetProtocol, @dataTime)";
            using (var command = new MySqlCommand(quary, connection))
            {
                command.Parameters.AddWithValue("@internetProtocol", internetProtocol);
                command.Parameters.AddWithValue("@dataTime", dateTime);
                command.ExecuteNonQuery();
            }
            CloseConecton();
        }

        public void InsertUser(string Name, string First_Name, string Patronymic, string Pass, string Login)
        {
            OpenConection();
            string quary = "INSERT INTO Users (Name, First_Name,Patronymic,Pass,Login) VALUES (@Name, @First_Name,@Patronymic,@Password,@Login)";
            using (var command = new MySqlCommand(quary, connection))
            {
                command.Parameters.AddWithValue("@Name", Name);
                command.Parameters.AddWithValue("@First_Name", First_Name);
                command.Parameters.AddWithValue("@Patronymic", Patronymic);
                command.Parameters.AddWithValue("@Password", Pass);
                command.Parameters.AddWithValue("@Login", Login);
                command.ExecuteNonQuery();
            }

            CloseConecton();
        }


        public string[] GetSignInUser(string Loggin, string pass)
        {
            OpenConection();

            string query = "SELECT Login, Pass FROM Users WHERE Login = @Login and Pass = @Password";
            string[] LogPass = new string[2];
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Login", Loggin);
                command.Parameters.AddWithValue("@Password", pass);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string Login = reader.GetString(0);
                        string Password = reader.GetString(1);
                        for (int i = 0; i < 2; i++)
                        {
                            if (i==0)
                            {
                                LogPass[i] = Login;
                            }
                            if (i == 1)
                            {
                                LogPass[i] = Password;
                            }
                            
                        }
                        
                    }
                    return LogPass;
                }
                CloseConecton();
            }
            
        }

        public string[] GetLogs()
        {
            OpenConection();
            string[] Log = new string[3];
            string query = "SELECT Internet_Protocol,Date,Methot FROM Logs WHERE Id_Logs=1";
            
            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        
                        string Internet_Protocol = reader.GetString(0);
                        DateTime Date = reader.GetDateTime(1);
                        string Method = reader.GetString(2);
                        
                        Log[0] = Internet_Protocol;
                        Log[1] = Date.ToString();
                        Log[2] = Method;

                    }
                    return Log;

                }
                CloseConecton();
            }
        }

        public void DeleteUser(string login)
        {
            OpenConection();

            string query = "DELETE FROM Users WHERE Login = @login";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@login", login);

                try
                {
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    { 
                        Console.WriteLine($"\nПользователь с логином '{login}' успешно удален из таблицы Users.");
                    }
                    else
                    {
                        Console.WriteLine($"\nПользователь с логином '{login}' не найден в таблице Users.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nОшибка 0x12 при удалении пользователя из Users: {ex.Message}!");
                }
            }

            CloseConecton();
        }

        public void SaveLogsToDatabase()
        {
            OpenConection();

            
            LogAggregator logAggregator = new LogAggregator("Apach24/logs/access.log");

            var logEntries = logAggregator.ReadLogs();

            foreach (var entry in logEntries)
            {
                if (entry.Url.Length <= 100 && entry.Method.Length <= 100)
                {
                    var query = "INSERT INTO Logs (Internet_Protocol, Date, Methot, url) VALUES (@ip, @dateTime, @method, @url)";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ip", entry.IP);
                        command.Parameters.AddWithValue("@dateTime", entry.DateTime);
                        command.Parameters.AddWithValue("@method", entry.Method);
                        command.Parameters.AddWithValue("@url", entry.Url);

                        command.ExecuteNonQuery();
                    }
                    Console.WriteLine("Успешно");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nОшибка 0x3: длина столбцов Url и Method должна быть не больше 100 символов!");
                }
            }

            CloseConecton();
        }
    }
}
