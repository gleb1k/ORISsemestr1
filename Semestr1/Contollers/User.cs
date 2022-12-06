using Semestr1.ORM;
using System.Text;
using Semestr1.Attributes;
using System.Net;
using Semestr1.Extensions;

namespace Semestr1.Contollers
{
    [HttpController("user")]
    public class User
    {
        [HttpGET("profile")]
        public static async Task ShowProfile(HttpListenerContext context)
        {
            var cookie = context.Request.Cookies["session-id"];
            if (cookie == null)
            {
                await context.ShowError(401, "Вы должны войти в аккаунт, чтобы посмотреть свой профиль!");
                return;
            }

            var sessionId = Convert.ToInt32(context.Request.Cookies["session-id"]?.Value);
            var user = UserDAO.GetById(sessionId);
            if (user == null)
            {
                await context.ShowError(404, "User doesn't found");
                return;
            }
            await ScribanMethods.GenerateProfilePage(user.GetNormalModel());
            await context.ShowPage(@"\profile\profile.html");
        }

        [HttpPOST("updatePOST")]
        public static async Task Update(HttpListenerContext context)
        {
            var cookie = context.Request.Cookies["session-id"];
            //просрочилась сессия
            if (cookie == null)
            {
                await context.ShowError(440, "Сессия просрочена");
                return;
            }
            var dict = context.GetBodyData();
            if (dict.CheckEmptyness())
            {
                //user id
                var sessionId = Convert.ToInt32(context.Request.Cookies["session-id"]?.Value);

                var age = Convert.ToInt32(dict["Age"]);
                var mobile = dict["Mobile"];
                var username = dict["Username"];
                var avatarUrl = dict["AvatarUrl"];
                var user = UserDAO.UpdateUser(sessionId,username, age, mobile,avatarUrl);
                if (user != null)
                {
                    context.Response.Redirect(@"http://localhost:8800/user/profile");
                    return;
                }
            }
            else
            {
                await context.ShowError(400, "Заполните поля!");
                return;
            }

            await context.ShowError(500, "Не удалось обновить данные на сервере!");
        }

        [HttpGET("signout")]
        public static async Task SignOut(HttpListenerContext context)
        {
            context.DeleteCookie("session-id");
            context.Response.Redirect(@"http://localhost:8800/anime/home");
        }
    }
}