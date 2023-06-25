using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleManagerDB
{
    public class user
    {
        public string Loggin { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }

    }
    public class ProgramAuse
    {
        
        public static void Hi()
        {
            
            Console.WriteLine("Добро пожаловать в приложение\n");
            

        }
        public int Action()
        {
            Console.WriteLine();
            Console.WriteLine("Выберете действие\n" +
                "1) Зарегестрироваться\n" +
                "2) Авторизироватся\n" +
                "Введите цыфру: ");

            int a = Convert.ToInt32(Console.ReadLine());
            if (a==1 || a == 2)
            {
                Console.Clear();
                return a;
            }
            else
            {
                Action();
                return 0;
            }
            
            
           
        }

        public static void Regestation()
        {
            Console.WriteLine();

            DataBase db = new DataBase();

            Console.WriteLine("Введите ваше имя");
            string Name = Console.ReadLine();
            Console.WriteLine("Введите вашу фамилию");
            string FirstName = Console.ReadLine();
            Console.WriteLine("Введите ваше отчество");
            string Patranimyc = Console.ReadLine();
            Console.WriteLine("Введите новый пароль");
            string Pass = Console.ReadLine();
            Console.WriteLine("Введите новый Login");
            string Login = Console.ReadLine();


            db.InsertUser(Name, FirstName, Patranimyc, Pass, Login);
            user user = new user();
            user.Name = Name;
            user.FirstName = FirstName;
            user.Patronymic = Patranimyc;
            user.Password = Pass;
            user.Loggin = Login;
            Console.Clear();
            Console.WriteLine("Вы успешно зарегестрировались и вошли в аккаунт");
            
        }

        public static void SignIn()
        {
           
            Console.WriteLine("Введите Login");
            string Login = Console.ReadLine();
            Console.WriteLine("Введите пароль");
            string Pass = Console.ReadLine();

            DataBase db = new DataBase();
            string[] mass =  db.GetSignInUser(Login,Pass);

            user user = new user();
            user.Loggin = mass[0];
            user.Password = mass[1];
            Console.Clear();
        }

        public void Program()
        {
            Hi();
            int a = Action();
            
            if (a ==1)
            {
                 Regestation();
                ComandHelp();
            }
            else if(a == 2)
            {
                SignIn();
                ComandHelp();
            }
           
        }
        public int Help()
        {
            Console.WriteLine("Введите /Help для просмотра команд");
            string help = Console.ReadLine();
            if (help == "/help" || help == "/Help")
            {
                Console.WriteLine("Это приложение может выполнять такие команды как:\n" +
                    "1) Добавление нового пользователя\n" +
                    "2) Вывод данных в Json файл\n" +
                    "3) Удаление пользователя\n" +
                    "4) Добавление log в базу данных\n" +
                    "5) Завершить программу\n" +
                    "Введите число: ");
                int a = Convert.ToInt32(Console.ReadLine());

                return a;
                
            }
            else if(Convert.ToInt32(help) >1 && Convert.ToInt32(help)<=5)
            {
                Console.Clear();
                return Convert.ToInt32(help);

            }
            else
            {
                
                Console.WriteLine("Такой команды нет");
                return 0;
            }
            
        }
        public void ComandHelp()
        {
            DataBase dataBase = new DataBase();
            Programs programs = new Programs();
            int a = Help();
            switch (a)
            {
                case 1:
                    Console.Clear();
                    Regestation();
                    Help();
                    break;
                case 2:
                    Console.Clear();
                    programs.Write();
                    Help();
                    break;
                case 3:
                    Console.Clear();
                    Console.WriteLine("Введите пользователя:\n");
                    string user = Console.ReadLine();
                    
                    dataBase.DeleteUser(user);
                    Help();
                    break;

                case 4:
                    Console.Clear();
                    dataBase.SaveLogsToDatabase();
                    Help();
                    break;
                case 5:
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Некоректное число");
                    Help();
                    break;

            }
        }

    }
}
