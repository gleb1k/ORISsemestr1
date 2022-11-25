using Semestr1;
using Semestr1.Models;
using Semestr1.ORM;
using Semestr1.Server;
using Semestr1.Contollers;
using Semestr1.Attributes;

    //var temp = ScribanMethods.GenerateProfile(@"\templates\profile.html", new User(228, "login", "password", 18, "+8938232342", 1337));

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
