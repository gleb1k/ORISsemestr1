using Semestr1.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Semestr1.Extensions
{
    public static class Extensions
    {
        private const string PublicFolder = "site";
        private static readonly string PublicFolderPath = Path.Join(Directory.GetCurrentDirectory(), PublicFolder);

        public static async Task ServerPage(this HttpListenerContext context, string path)
        {
            var fullPath = Path.Join(PublicFolderPath, path);
            if (!File.Exists(fullPath))
            {
                context.Response.StatusCode = 404;
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
            await context.Response.OutputStream.WriteAsync(Encoding.UTF8.GetBytes("<h2>404<h2><h3>The resource can not be found :c<h3>"));
        }
    }
}
