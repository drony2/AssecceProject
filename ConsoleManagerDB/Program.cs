
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;

namespace ConsoleManagerDB
{
    internal class Program
    {

        static void Main(string[] args)
        {
            ProgramAuse programAuse = new ProgramAuse();
            
            programAuse.Program();



            Console.ReadKey();
        }

    }

}
