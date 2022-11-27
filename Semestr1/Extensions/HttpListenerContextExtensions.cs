using Semestr1.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Semestr1.Extensions
{
    public static class Extensions
    {
        private const string PublicFolder = "site";
        private static readonly string PublicFolderPath = Path.Join(Directory.GetCurrentDirectory(), PublicFolder);

        public static async Task ShowPage(this HttpListenerContext context, string path)
        {
            var fullPath = Path.Join(PublicFolderPath, path);
            if (!File.Exists(fullPath))
            {
                await context.Show404();
            }
            else
            {
                context.Response.StatusCode = 200;
                context.Response.ContentType = Path.GetExtension(path) switch
                {
                    ".js" => "application/javascript",
                    ".css" => "text/css",
                    ".html" => "text/html",
                    ".png" => "image/png",
                    ".jpg" => "image/jpeg",
                    ".jpeg" => "image/jpeg",
                    ".gif" => "image/gif",
                    _ => "text/plain"
                };
                var file = await File.ReadAllBytesAsync(fullPath);
                await context.Response.OutputStream.WriteAsync(file);
            }
        }

        public static async Task Show404(this HttpListenerContext context)
        {
            context.Response.ContentType = "text/html; charset=utf-8";
            context.Response.StatusCode = 404;
            await context.Response.OutputStream.WriteAsync(
                Encoding.UTF8.GetBytes("<h2>404<h2><h3>The resource can not be found :c<h3>"));
        }

        public static async Task ShowSessionExpired(this HttpListenerContext context)
        {
            context.Response.ContentType = "text/html; charset=utf-8";
            context.Response.StatusCode = 440;
            await context.Response.OutputStream.WriteAsync(
                Encoding.UTF8.GetBytes("<h2>440<h2><h3>Your session has expired and you must log in again.<h3>"));
        }
        
        /// <summary>
        /// parcing data from request to dict
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetBodyData(this HttpListenerContext context)
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

        public static void AddCookie(this HttpListenerContext context, string name, string value, double lifetime)
        {
            context.Response.Cookies.Add(new Cookie
            {
                Name = name,
                Value = value,
                Path = "/",
                //кука будет жить lifetime минут, после этого сессия закончится и пользователю нужно будет реавторизироваться
                Expires = DateTime.UtcNow.AddMinutes(lifetime)
            });
        }

        public static void DeleteCookie(this HttpListenerContext context, string name)
        {
            if (context.Request.Cookies[name] != null)
            {
                context.Response.Cookies.Add(new Cookie
                {
                    Name = name,
                    Path = "/",
                    //кука будет жить lifetime минут, после этого сессия закончится и пользователю нужно будет реавторизироваться
                    Expires = DateTime.UtcNow.AddDays(-1)
                });
            }
        }
    }
}