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

namespace Semestr1.Contollers
{
    [HttpController("users")]
    public class Users
    {
        [HttpGET("getuser")]
        public static User GetUser(HttpListenerContext context)
        {
            throw new NotImplementedException();
        }

        [HttpPOST("updatePOST")]
        public static bool? Update(HttpListenerContext context)
        {
            //string login = "";
            //string password = "";
            //var user = new User(login, password, age, mobile, favoriteAnime);
            //throw new NotImplementedException();
            throw new NotImplementedException();
        }

        [HttpPOST("registerPOST")]
        public static async Task Register(HttpListenerContext context)
        {
            var dict = GetBodyData(context);

            if (dict != null)
            {
                var temp = dict["Login"];
                var temp2 = dict["Password"];
                //должно быть асинхронно, но тяжело
                var result = UserDAO.AddUser(dict["Login"], dict["Password"]);
                if (result != null)
                {
                    context.Response.StatusCode = 204;
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
                //должно быть асинхронно, но тяжело
                var result = UserDAO.Login(dict["Login"], dict["Password"]);
                if (result!=null)
                {
                    context.Response.StatusCode = 204;
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
