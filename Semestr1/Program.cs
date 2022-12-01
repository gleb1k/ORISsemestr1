using Scriban;
using Semestr1.Contollers;
using Semestr1.Models;
using Semestr1.ORM;
using Semestr1.Server;


string html = "{{post.user.username}} and {{post.anime.name}}";

//Parse a scriban template
var template = Template.Parse(html);

var post = PostDAO.GetById(5).GetNormalModel();

var result = template.RenderAsync(new { post = post});

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
