using Scriban;
using Semestr1.Contollers;
using Semestr1.Models;
using Semestr1.Server;

// var myOrm = new MyORM(@"Host=localhost;Username=postgres;Password=12345678;Database=AnimeDB");
// string nonQuery = $"insert into Posts (authorid, animeid) " +
//                   $"VALUES " +
//                   $"(" +
//                   $"'1'," +
//                   $"'1'" +
//                   $")  RETURNING id";
// var test = myOrm.ExecuteScalar<int>(nonQuery);

string html = "{{user.username}} and {{anime.name}}";

// Parse a scriban template
// var template = Template.Parse(html);
// var user = new UserModel();
// user.Username = "user123123";
// var anime = new AnimeModel();
// anime.Name = "animecool";
//
// var result = template.RenderAsync(new { user = user, anime = anime });

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
