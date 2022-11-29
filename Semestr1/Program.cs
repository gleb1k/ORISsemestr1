using Semestr1.Server;

// var myOrm = new MyORM(@"Host=localhost;Username=postgres;Password=12345678;Database=AnimeDB");
// string nonQuery = $"insert into Posts (authorid, animeid) " +
//                   $"VALUES " +
//                   $"(" +
//                   $"'1'," +
//                   $"'1'" +
//                   $")  RETURNING id";
// var test = myOrm.ExecuteScalar<int>(nonQuery);

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
