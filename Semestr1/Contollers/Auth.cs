using System.Net;
using System.Text;
using System.Web;
using Semestr1.Attributes;
using Semestr1.Extensions;
using Semestr1.ORM;

namespace Semestr1.Contollers;

[HttpController("auth")]
public class Auth
{
    [HttpGET("register")]
    public static async Task ShowRegister(HttpListenerContext context)
    {
        var cookie = context.Request.Cookies["session-id"];
        if (cookie == null)
        {
            context.Response.StatusCode = 200;
            await context.ShowPage(@"\register\register.html");
            return;
        }
        context.Response.StatusCode = 305;
        context.Response.Redirect(@"http://localhost:8800/user/profile");
    }

    [HttpGET("login")]
    public static async Task ShowLogin(HttpListenerContext context)
    {
        var cookie = context.Request.Cookies["session-id"];
        if (cookie == null)
        {
            context.Response.StatusCode = 200;
            await context.ShowPage(@"\login\login.html");
            return;
        }
        context.Response.StatusCode = 305;
        context.Response.Redirect(@"http://localhost:8800/user/profile");
    }

    [HttpPOST("registerPOST")]
    public static async Task Register(HttpListenerContext context)
    {
        var dict = context.GetBodyData();
        if (dict != null)
        {
            var user = UserDAO.Add(dict["Login"], dict["Password"]);
            if (user != null)
            {
                context.AddCookie("session-id", user.Id.ToString(), 20d);
                context.Response.Redirect(@"http://localhost:8800/user/profile");
                return;
            }
        }

        context.Response.StatusCode = 500;
        context.Response.ContentType = "text/plain; charset=utf-8";
        context.Response.OutputStream.Write(Encoding.UTF8.GetBytes("Передача данных на сервер не удалась!"));
    }

    [HttpPOST("loginPOST")]
    public static async Task Login(HttpListenerContext context)
    {
        var dict = context.GetBodyData();
        if (dict != null)
        {
            var user = UserDAO.Get(dict["Login"], dict["Password"]);
            if (user != null)
            {
                context.AddCookie("session-id", user.Id.ToString(), 20d);
                context.Response.Redirect(@"http://localhost:8800/user/profile");
                return;
            }
        }

        context.Response.StatusCode = 500;
        context.Response.ContentType = "text/plain; charset=utf-8";
        context.Response.OutputStream.Write(Encoding.UTF8.GetBytes("Передача данных на сервер не удалась!"));
    }
}