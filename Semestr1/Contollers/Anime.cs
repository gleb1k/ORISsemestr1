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
    [HttpController("anime")]
    public partial class Anime
    {
        [HttpGET("getuser")]
        public static User GetUser(HttpListenerContext context)
        {
            throw new NotImplementedException();
        }

        [HttpPOST("updatePOST")]
        public static async Task Update(HttpListenerContext context)
        {
            var dict = GetBodyData(context);
                
            if (dict != null)
            {
                //user id
                var sessionId = Convert.ToInt32(context.Request.Cookies["session-id"]?.Value);
                //просрочилась сессия
                if (sessionId == null)
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "text/plain; charset=utf-8";
                    context.Response.OutputStream.Write(
                        Encoding.UTF8.GetBytes("Закончилась сессия!"));
                    return;
                }
                
                var age = Convert.ToInt32(dict["Age"]);
                var mobile = dict["Mobile"];
                //должно быть асинхронно, но тяжело
                var user = UserDAO.UpdateUser(sessionId, age, mobile);
                
                if (user != null)
                {
                    context.Response.StatusCode = 204;
                    ScribanMethods.GenerateProfile(@"\templates\profile.html", user);
                    await context.ServerPage(@"\profile\profile.html");
                    return;
                }
            }
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/plain; charset=utf-8";
            context.Response.OutputStream.Write(Encoding.UTF8.GetBytes("Не удалось обновить данные на сервере!"));
        }

        [HttpPOST("registerPOST")]
        public static async Task Register(HttpListenerContext context)
        {
            var dict = GetBodyData(context);

            if (dict != null)
            {
                //должно быть асинхронно, но тяжело
                var user = UserDAO.AddUser(dict["Login"], dict["Password"]);
                if (user != null)
                {
                    context.Response.Cookies.Add(new Cookie
                    {
                        Name = "session-id",
                        Value = user.Id.ToString(),
                        // Port = "8800",
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

        [HttpPOST("loginPOST")]
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

        /// <summary>
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
}