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
        if (context.isAuthorized())
        {
            context.Response.StatusCode = 305;
            context.Response.Redirect(@"http://localhost:8800/user/profile");
        }
        else
        {
            context.Response.StatusCode = 200;
            await context.ShowPage(@"\register\register.html");
        }
    }

    [HttpGET("login")]
    public static async Task ShowLogin(HttpListenerContext context)
    {
        if (context.isAuthorized())
        {
            context.Response.StatusCode = 305;
            context.Response.Redirect(@"http://localhost:8800/user/profile");
        }
        else
        {
            context.Response.StatusCode = 200;
            await context.ShowPage(@"\login\login.html");
        }
    }

    [HttpPOST("registerPOST")]
    public static async Task Register(HttpListenerContext context)
    {
        //уже авторизирован
        if (context.isAuthorized())
        {
            context.Response.Redirect(@"http://localhost:8800/user/profile");
            return;
        }
        var dict = context.GetBodyData();
        if (dict.CheckEmptyness() && Validation.CheckLoginAndPassword(dict["Login"], dict["Password"]))
        {
            var user = UserDAO.Add(dict["Login"], dict["Password"]);
            if (user != null)
            {
                if (dict.ContainsKey("Checkbox"))
                    context.AddCookieForOneDay("session-id", user.Id.ToString());
                else
                    context.AddCookie("session-id", user.Id.ToString(), 20d);
                context.Response.Redirect(@"http://localhost:8800/user/profile");
                return;
            }
        }
        else
        {
            await context.ShowError(400, "Невалидные данные");
            return;
        }
        await context.ShowError(500, "Не удалось обновить данные на сервере!");
    }

    [HttpPOST("loginPOST")]
    public static async Task Login(HttpListenerContext context)
    {
        //уже авторизирован
        if (context.isAuthorized())
        {
            context.Response.Redirect(@"http://localhost:8800/user/profile");
            return;
        }
        var dict = context.GetBodyData();
        if (dict.CheckEmptyness() && Validation.CheckLoginAndPassword(dict["Login"], dict["Password"]))
        {
            var user = UserDAO.Get(dict["Login"], dict["Password"]);
            if (user != null)
            {
                if (dict.ContainsKey("Checkbox"))
                    context.AddCookieForOneDay("session-id", user.Id.ToString());
                else
                    context.AddCookie("session-id", user.Id.ToString(), 20d);

                context.Response.Redirect(@"http://localhost:8800/user/profile");
                return;
            }
        }
        else
        {
            await context.ShowError(400, "Невалидные данные");
            return;
        }
        await context.ShowError(500, "Не удалось обновить данные на сервере!");
    }
}