using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace EFGetStarted
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DbCommands.InitializeDB(forceReset: false);

            //// CRUD (Create-Read-Update-Delete)
            Console.WriteLine("명령어를 입력하세요");
            Console.WriteLine("[0] Force Reset");
            Console.WriteLine("[1] Eager Loading");
            Console.WriteLine("[2] Explicit Loading");
            Console.WriteLine("[3] Select Loading");

            while (true)
            {
                Console.Write("> ");
                string command = Console.ReadLine();
                switch (command)
                {
                    case "0":
                        DbCommands.InitializeDB(forceReset: true);
                        break;
                    case "1":
                        DbCommands.EagerLoading();
                        break;
                    case "2":
                        DbCommands.ExplicitLoading();
                        break;
                    case "3":
                        DbCommands.SelectLoading();

                        break;
                }
            }
        }
    }
}
