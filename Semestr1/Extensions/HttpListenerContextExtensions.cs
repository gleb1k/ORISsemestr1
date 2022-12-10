using System.Net;
using System.Text;
using System.Web;

namespace Semestr1.Extensions
{
    public static class HttpListenerContextExtensions
    {
        private const string PublicFolder = "site";
        private static readonly string PublicFolderPath = Path.Join(Directory.GetCurrentDirectory(), PublicFolder);

        public static async Task ShowPage(this HttpListenerContext context, string path)
        {
            var fullPath = Path.Join(PublicFolderPath, path);
            if (!File.Exists(fullPath))
            {
                await context.ShowError(404, "Ресурс не найден");
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

        public static async Task ShowError(this HttpListenerContext context, int statusCode, string message)
        {
            context.Response.ContentType = "text/html; charset=utf-8";
            context.Response.StatusCode = statusCode;
            await context.Response.OutputStream.WriteAsync(
                Encoding.UTF8.GetBytes($"<p>{statusCode}<p><p>{message}<p>"));
        }
        
        /// <summary>
        /// parsing data from request to dictionary
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

        public static bool CheckCookie(this HttpListenerContext context, string name)
        {
            var cookie = context.Request.Cookies[name];
            if (cookie == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
        public static bool isAuthorized(this HttpListenerContext context)
        {
            var cookie = context.Request.Cookies["session-id"];
            if (cookie == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static void AddCookieForOneDay(this HttpListenerContext context, string name, string value)
        {
            context.Response.Cookies.Add(new Cookie
            {
                Name = name,
                Value = value,
                Path = "/",
                //кука будет жить lifetime минут, после этого сессия закончится и пользователю нужно будет реавторизироваться
                Expires = DateTime.UtcNow.AddDays(1d)
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
                    Expires = DateTime.UtcNow.AddDays(-1)
                });
            }
        }
    }
}