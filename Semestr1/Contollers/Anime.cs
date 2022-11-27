using System.Net;
using System.Text;
using Semestr1.Attributes;
using Semestr1.Extensions;
using Semestr1.ORM;

namespace Semestr1.Contollers
{
    [HttpController("anime")]
    public class Anime
    {
        [HttpGET("home")]
        public static async Task ShowHome(HttpListenerContext context)
        {
            var animes = AnimeDAO.GetAll();
            await ScribanMethods.GenerateHomePage(@"\home\home.html", animes);
            await context.ShowPage(@"\home\home.html");
        }

        [HttpPOST("addAnimePOST")]
        public static async Task AddAnime(HttpListenerContext context)
        {
            var dict = context.GetBodyData();
            if (dict != null)
            {
                var anime = AnimeDAO.Add(dict["Name"], dict["Author"], dict["Description"]);
                if (anime != null)
                {
                    context.Response.Redirect(@"http://localhost:8800/anime/home");
                    return;
                }
            }

            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/plain; charset=utf-8";
            await context.Response.OutputStream.WriteAsync(Encoding.UTF8.GetBytes("Передача данных на сервер не удалась!"));
        }
    }
}