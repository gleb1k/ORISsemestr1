using Scriban;
using Semestr1.Contollers;
using Semestr1.Models;
using Semestr1.ORM;
using Semestr1.Server;


// string html = "";
//
// //Parse a scriban template
// var template = Template.Parse(html);
//
//
// var result = template.RenderAsync(new { i_item = "https://www.youtube.com/watch?v=IAEim-jKLMA&list=PLzSIvl7IRPWThD1dd-cwQlIYXKNVoB4rS&index=25"});
// var temp = result.Result;
var orm = new MyORM(ServerSettings._connectionString);
var query = "select * from users";
orm.ExecuteQuery<UserModel>(query);
orm.ExecuteQuery<UserModel>(query);


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
