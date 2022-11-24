using Semestr1.Models;
using Semestr1.ORM;
using Semestr1.Server;
using Semestr1.Contollers;
using Semestr1.Attributes;

var myorm = new MyORM(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AnimeDB;Integrated Security=True");
//var anime = new Anime(1, "animeTest", "huysuy", "anime super good", "");
//var temp = myorm.ExectureScalar<User>(new User(1,"login","password",13,"8983834834",1));
//var list = myorm.ExectureScalar<User>("select * from Users");
//myorm.Insert<User>(new User(343, "loginnew", "passwordnew","18", "8987858594", "boku no pico"));
var temp = new User(343, "loginnew", "passwordnew", null, "8987858594", null);

//var accRep = new AccountRepository<Account>();

//Настройка сервера

bool _appIsRunning = true;
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
