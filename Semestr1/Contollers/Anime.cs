using System.Net;
using System.Text;
using System.Xml.Xsl;
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
            if (!context.CheckCookie("session-id"))
            {
                await context.ShowError(440, "Вы не авторизированы");
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
                var animeImageUrl = dict["ImageUrl"];

                var anime = AnimeDAO.Add(animeName, animeDescription, animeGenre, animeStudio, animeAgeRating, animeImageUrl);
                
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
                await context.ShowError(400, "Заполните поля!");
                return;
            }
            await context.ShowError(500, "Не удалось обновить данные на сервере!");
        }
        
        [HttpPOST("addtofavoritePOST")]
        public static async Task AddAnimeToFavorite(HttpListenerContext context)
        {
            if (!context.CheckCookie("session-id"))
            {
                await context.ShowError(440, "Вы не авторизированы");
                return;
            }
        }
    }
}