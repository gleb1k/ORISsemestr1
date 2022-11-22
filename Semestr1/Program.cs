using Semestr1.Models;
using Semestr1.ORM;
using Semestr1.Server;
using Semestr1.Contollers;
using Semestr1.Attributes;

bool _appIsRunning = true;
var myorm = new MyORM(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AnimeDB;Integrated Security=True");

//myorm.Insert<User>(new User(343, "loginnew", "passwordnew","18", "8987858594", "boku no pico"));

//var accRep = new AccountRepository<Account>();

//Настройка сервера
new ServerSettings().Serialize();

//Запуск сервера
var httpserver = new HttpServer();
using (httpserver)
{
    httpserver.Start();

    while (_appIsRunning)
    {
        Handler(Console.ReadLine()?.ToLower(), httpserver);
    }
}
void Handler(string command, HttpServer httpserver)
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
