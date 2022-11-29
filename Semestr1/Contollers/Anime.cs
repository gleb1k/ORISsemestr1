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
            await ScribanMethods.GenerateHomePage(animes);
            await context.ShowPage(@"\home\home.html");
        }

        [HttpPOST("addAnimePOST")]
        public static async Task AddAnime(HttpListenerContext context)
        {
            var dict = context.GetBodyData();
            if (dict.CheckEmptyness())
            {
                var anime = AnimeDAO.Add(dict["Name"], dict["Author"], dict["Description"]);
                if (anime != null)
                {
                    context.Response.Redirect(@"http://localhost:8800/anime/home");
                    return;
                }
            }
            else
            {
                //something wasn't filled
                context.Response.StatusCode = 400;
                context.Response.ContentType = "text/plain; charset=utf-8";
                context.Response.OutputStream.Write(Encoding.UTF8.GetBytes("Заполните поля!"));
                return;
            }

            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/plain; charset=utf-8";
            await context.Response.OutputStream.WriteAsync(
                Encoding.UTF8.GetBytes("Передача данных на сервер не удалась!"));
        }
        [HttpPOST("addPostPOST")]
        public static async Task AddPost(HttpListenerContext context)
        {
            var dict = context.GetBodyData();
            if (dict.CheckEmptyness())
            {
                //todo
                var post = PostDAO.Add(Convert.ToInt32(dict["UserId"]), Convert.ToInt32(dict["AnimeId"]));
                if (post != null)
                {
                    context.Response.Redirect(@"http://localhost:8800/anime/home");
                    return;
                }
            }
            else
            {
                //something wasn't filled
                context.Response.StatusCode = 400;
                context.Response.ContentType = "text/plain; charset=utf-8";
                context.Response.OutputStream.Write(Encoding.UTF8.GetBytes("Заполните поля!"));
                return;
            }

            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/plain; charset=utf-8";
            await context.Response.OutputStream.WriteAsync(
                Encoding.UTF8.GetBytes("Передача данных на сервер не удалась!"));
        }
    }
}