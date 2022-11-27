using Semestr1.Models;
using Semestr1.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semestr1.Attributes;
using Semestr1.Server;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Text.Json;
using System.Web;
using Semestr1.Extensions;
using StackExchange.Redis;

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
                context.Response.StatusCode = 401;
                context.Response.ContentType = "text/plain; charset=utf-8";
                context.Response.OutputStream.Write(
                    Encoding.UTF8.GetBytes("Вы должны войти в аккаунт, чтобы посмотреть свой профиль!"));
                return;
            }

            var sessionId = Convert.ToInt32(context.Request.Cookies["session-id"]?.Value);
            var user = UserDAO.GetById(sessionId);
            await ScribanMethods.GenerateProfilePage(@"\profile\profile.html", user);
            await context.ShowPage(@"\profile\profile.html");
        }

        [HttpPOST("updatePOST")]
        public static async Task Update(HttpListenerContext context)
        {
            var cookie = context.Request.Cookies["session-id"];
            //просрочилась сессия
            if (cookie == null)
            {
                await context.ShowSessionExpired();
                return;
            }
            var dict = context.GetBodyData();
            if (dict != null)
            {
                //user id
                var sessionId = Convert.ToInt32(context.Request.Cookies["session-id"]?.Value);

                var age = Convert.ToInt32(dict["Age"]);
                var mobile = dict["Mobile"];
                var user = UserDAO.UpdateUser(sessionId, age, mobile);
                if (user != null)
                {
                    context.Response.Redirect(@"http://localhost:8800/user/profile");
                    return;
                }
            }

            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/plain; charset=utf-8";
            context.Response.OutputStream.Write(Encoding.UTF8.GetBytes("Не удалось обновить данные на сервере!"));
        }

        [HttpGET("signout")]
        public static async Task SignOut(HttpListenerContext context)
        {
            context.DeleteCookie("session-id");
            context.Response.Redirect(@"http://localhost:8800/anime/home");
        }
    }
}