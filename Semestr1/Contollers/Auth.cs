using System.Net;
using System.Text;
using System.Web;
using Semestr1.Attributes;
using Semestr1.Extensions;
using Semestr1.ORM;

namespace Semestr1.Contollers;

[HttpController("Auth")]
public class Auth
{
    [HttpPOST("authPOST")]
    public static async Task Login(HttpListenerContext context)
    {
        var dict = GetBodyData(context);
        if (dict != null)
        {
            var user = UserDAO.GetUser(dict["Login"], dict["Password"]);
            if (user != null)
            {
                // var randomBytes = new byte[32];
                // new Random().NextBytes(randomBytes);
                // var sessionId = Convert.ToBase64String(randomBytes);

                //Добавляю в редис на 4 часа (не работает)
                // await RedisStore.RedisCache.StringSetAsync(sessionId, user.Id.ToString(), TimeSpan.FromHours(4));
                context.Response.Cookies.Add(new Cookie
                {
                    Name = "session-id",
                    Value = user.Id.ToString(),
                    // Port = "8800",
                    Path = "/",
                    //кука будет жить 20 минут, после этого сессия закончится и пользователю нужно будет реавторизироваться
                    Expires = DateTime.UtcNow.AddMinutes(20d)
                });
                ScribanMethods.GenerateProfile(@"\templates\profile.html", user);
                await context.ServerPage(@"\profile\profile.html");
                return;
            }
        }
        context.Response.StatusCode = 500;
        context.Response.ContentType = "text/plain; charset=utf-8";
        context.Response.OutputStream.Write(Encoding.UTF8.GetBytes("Передача данных на сервер не удалась!"));
    }
    /// parcing data from request to dict
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private static Dictionary<string, string> GetBodyData(HttpListenerContext context)
    {
        var request = context.Request;
        if (!request.HasEntityBody)
        {
            return null;
        }

        Stream body = request.InputStream;
        Encoding encoding = request.ContentEncoding;
        StreamReader reader = new StreamReader(body, encoding);

        var data = HttpUtility.ParseQueryString(reader.ReadToEnd());
        body.Close();
        reader.Close();

        var dataDict = data.ToDictionary();
        return dataDict;
    }
}