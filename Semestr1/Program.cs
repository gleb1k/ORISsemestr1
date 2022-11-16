using Semestr1.Models;
using Semestr1.ORM;
using Semestr1.Server;

namespace HttpServer
{
    internal class Program
    {

        private static bool _appIsRunning = true;
        static void Main(string[] args)
        {
            var myorm = new MyORM(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SteamDB;Integrated Security=True");
            ////var list = myorm.Select<Account>();
            myorm.Insert<Account>(new Account(343, "loginnew", "passwordnew", "8987858594"));

            //var accRep = new AccountRepository<Account>();

            ////--Работа с найстройками сервера (сериализация и десериализация json)--
            //var settings = new ServerSettings();
            //settings.Serialize();
            //var settingsDeserialized = ServerSettings.Deserialize();

            //Запуск сервера
            var httpserver = new HttpServer.HttpServer();
            using (httpserver)
            {
                httpserver.Start();

                while (_appIsRunning)
                {
                    Handler(Console.ReadLine()?.ToLower(), httpserver);
                }
            }
        }
        static void Handler(string command, HttpServer httpserver)
        {
            switch (command)
            {
                case "stop":
                    httpserver.Stop();
                    break;
                case "start":
                    httpserver.Start();
                    break;
                case "restart":
                    httpserver.Stop();
                    httpserver.Start();
                    break;
                case "status":
                    Console.WriteLine(httpserver.Status);
                    break;
                case "exit":
                    _appIsRunning = false;
                    break;
            }
        }

    }
}
