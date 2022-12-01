using System.Net;
using System.Text;
using Semestr1.Attributes;
using Semestr1.Extensions;
using Semestr1.Models;
using Semestr1.ORM;

namespace Semestr1.Contollers
{
    [HttpController("anime")]
    public class Anime
    {
        [HttpGET("home")]
        public static async Task ShowHome(HttpListenerContext context)
        {
            var posts = PostDAO.GetAll();
            List<PostNormalModel> newPosts = new List<PostNormalModel>();
            foreach (var post in posts)
            {
                newPosts.Add(post.GetNormalModel());
            }
            
            await ScribanMethods.GenerateHomePage(newPosts);
            await context.ShowPage(@"\home\home.html");
        }
        [HttpPOST("addPostPOST")]
        public static async Task AddPost(HttpListenerContext context)
        {
            var cookie = context.Request.Cookies["session-id"];
            //просрочилась сессия
            if (cookie == null)
            {
                await context.ShowSessionExpired();
                return;
            }
            var dict = context.GetBodyData();
            if (dict.CheckEmptyness())
            {
                //user id
                var userId = Convert.ToInt32(context.Request.Cookies["session-id"]?.Value);

                var animeName = dict["Name"];
                var animeGenre = dict["Genre"];
                var animeStudio = dict["Studio"];
                var animeAgeRating = dict["AgeRating"];
                var animeDescription = dict["Description"];

                var anime = AnimeDAO.Add(animeName, animeDescription, animeGenre, animeStudio, animeAgeRating);
                
                if (anime != null)
                {

                    var postName = dict["PostName"];
                    var post = PostDAO.Add(postName,userId, anime.Id);

                    if (post != null)
                    {
                        context.Response.Redirect(@"http://localhost:8800/anime/home");
                        return;
                    }
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
            context.Response.OutputStream.Write(Encoding.UTF8.GetBytes("Не удалось обновить данные на сервере!"));
        }
        
        [HttpPOST("addtofavoritePOST")]
        public static async Task AddAnimeToFavorite(HttpListenerContext context)
        {
            var cookie = context.Request.Cookies["session-id"];
            //просрочилась сессия
            if (cookie == null)
            {
                await context.ShowSessionExpired();
                return;
            }
        }
    }
}