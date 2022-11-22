using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Semestr1.Server;

namespace Semestr1.Pages
{
    public class Pages
    {
        private static string SitePath = ServerSettings.Deserialize().SitePath;
        public static bool ShowHTMLPage(string directory, ref HttpListenerResponse response)
        {
            byte[] buffer = GetFile(directory);
            if (buffer == null)
            {
                return false;
            }
            //Files.GetExtension(ref response, directory);
            response.StatusCode = 201;
            response.ContentEncoding = Encoding.UTF8;
            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            //закрываем поток
            output.Close();
            return true;
        }
        /// <summary>
        /// Читает данные из папки. ПАПКА ДОЛЖНА НАЗЫВАТЬСЯ ТАКЖЕ КАК И HTML ДОКУМЕНТ!!
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        private static byte[] GetFile(string directory)
        {
            byte[] buffer = null;
            string filePath = $"{SitePath}{directory}/";
            if (Directory.Exists(filePath))
            {
                filePath = filePath + $"{directory}.html";
                if (File.Exists(filePath))
                {
                    buffer = File.ReadAllBytes(filePath);
                }
            }
            return buffer;
        }
        private static void GetExtension(ref HttpListenerResponse response, string directory)
        {
            string filePath = $"/{directory}/";
            response.ContentType = Path.GetExtension(filePath) switch
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
