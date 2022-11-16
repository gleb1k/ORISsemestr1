using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Semestr1.FileStuf
{
    public class Files
    {
        public static byte[] GetFile(string rawUrl)
        {
            byte[]? buffer = null;
            string filePath;
            //костыль
            if (rawUrl.Contains("/site") == false)
            {
                filePath = "./site" + rawUrl;
            }
            else
                filePath = "." + rawUrl;

            if (Directory.Exists(filePath))
            {
                //Каталог
                filePath = filePath + "/index.html";
                if (File.Exists(filePath))
                {
                    buffer = File.ReadAllBytes(filePath);
                }
            }
            else if (File.Exists(filePath))
            {
                //Файл
                buffer = File.ReadAllBytes(filePath);
            }
            return buffer;
        }

        public static void GetExtension(ref HttpListenerResponse response, string path)
        {
            //каталог?
            if (Directory.Exists(path))
            {
                path = path + "/index.html";
            }
            response.ContentType = Path.GetExtension(path) switch
            {
                ".html" => "text/html",
                ".png" => "image/png",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".svg" => "image/svg+xml",
                ".gif" => "image/gif",
                ".js" => "text/javascript",
                ".css" => "text/css",
                ".ico" => "image/x-icon",
                _ => "text/plain",
            };
        }
    }
}
