using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Semestr1.Server
{
    public class ServerSettings
    {
        static public string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AnimeDB;Integrated Security=True";
        public int Port { get; set; } = 8800;
        public string SitePath { get; set; } = @"./site/";
        public ServerSettings(int port, string path)
        {
            Port = port;
            SitePath = path;
        }
        public ServerSettings() { }

        public void Serialize()
        {
            //json serialization

            var jsonSerializer = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(this, jsonSerializer);

            using (StreamWriter streamWriter = new StreamWriter(@"Settings.JSON", false))
            {
                streamWriter.WriteLine(jsonString);
            }
        }
        public static ServerSettings Deserialize()
        {
            //json deserialization
            try
            {
                using (var fs = new FileStream(@"Settings.JSON", FileMode.OpenOrCreate))
                {
                    return JsonSerializer.Deserialize<ServerSettings>(fs);
                }
            }
            catch
            {
                Console.WriteLine("Settings doesn't found");
                return null;
            }
        }
    }
}
