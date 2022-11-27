using Semestr1.Attributes;
using Semestr1.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Semestr1.Server
{
    public class HttpServer : IDisposable
    {
        public ServerStatus Status = ServerStatus.Stop;

        private ServerSettings _serverSettings;
        private readonly HttpListener _httpListener;

        public HttpServer()
        {
            _serverSettings = ServerSettings.Deserialize();
            _httpListener = new HttpListener();
        }

        public void Start()
        {
            if (Status == ServerStatus.Start)
            {
                Console.WriteLine("Сервер уже запущен!");
            }
            else
            {
                Console.WriteLine("Запуск сервера...");

                _serverSettings = ServerSettings.Deserialize();
                _httpListener.Prefixes.Clear();
                _httpListener.Prefixes.Add($"http://localhost:" + _serverSettings.Port + "/");

                _httpListener.Start();
                Console.WriteLine("Ожидание подключений...");
                Status = ServerStatus.Start;
            }

            Listening();
        }

        public void Stop()
        {
            if (Status == ServerStatus.Start)
            {
                _httpListener.Stop();
                Status = ServerStatus.Stop;
                Console.WriteLine("Обработка подключений завершена");
            }
            else
                Console.WriteLine("Сервер уже остановлен");
        }

        private async void Listening()
        {
            while (_httpListener.IsListening)
            {
                var context = await _httpListener.GetContextAsync();

                var response = context.Response;
                _ = Task.Run(async () =>
                {
                    try
                    {
                        var method = GetMethodByContext(context);
                        if (method != null)
                        {
                            await (Task)method.Invoke(null, new object[] { context });
                        }
                        else
                        {
                            var temp = context.Request.Url.LocalPath;
                            await context.ServerPage(context.Request.Url.LocalPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        response.StatusCode = 500;
                    }

                    response.OutputStream.Close();
                    response.Close();
                });
            }
        }

        private Type? GetControllerByContext(HttpListenerContext context)
        {
            var request = context.Request;
            if (request.Url.Segments.Length < 2) return null;

            string controllerName = context.Request.Url.Segments[1].Replace("/", "");
            var assembly = Assembly.GetExecutingAssembly();

            // ищет контроллер ПО НАЗВАНИЮ КЛАССА. Т.Е НАЗВАНИЕ КЛАССА ДОЛЖНО БЫТЬ РАВНО НАЗВАНИЮ СТРОКИ ПЕРЕДАННОЙ В АТРИБУТ! говно
            var controller = assembly.GetTypes().Where(t => Attribute.IsDefined(t, typeof(HttpController)))
                .FirstOrDefault(c => c.Name.ToLower() == controllerName.ToLower());

            
            // var controller2 = assembly.GetTypes().Where(t => Attribute.IsDefined(t, typeof(HttpController)))
            //     .FirstOrDefault(c => c.Name.ToLower() == controllerName.ToLower());
            //попытка сделать адекватно
            //var classesWithHttpController = assembly.GetTypes().Where(t => Attribute.IsDefined(t, typeof(HttpController)));
            //var temp = classesWithHttpController.FirstOrDefault(c => c.CustomAttributes.FirstOrDefault(atr => atr.AttributeType.Name == "HttpController"));
            
            // var classesWithHttpController = assembly.GetTypes().Where(t => Attribute.IsDefined(t, typeof(HttpController)));
            // var temp = classesWithHttpController.FirstOrDefault(c => c.CustomAttributes.FirstOrDefault(atr => atr. == ""));
            return controller == null ? null : controller;
        }

        private MethodInfo GetMethodByContext(HttpListenerContext context)
        {
            var controller = GetControllerByContext(context);
            if (controller == null) return null;
            var request = context.Request;
            var methodURI = request.Url?.Segments[2].Replace("/", "");
            var methods = controller.GetMethods().Where(t => t.GetCustomAttributes(true)
                .Any(attr => attr.GetType().Name == $"Http{request.HttpMethod}"));
            //можно лучше
            var method = methods.FirstOrDefault(x => request.HttpMethod switch
            {
                "GET" => x.GetCustomAttribute<HttpGET>()?.MethodURI == methodURI,
                "POST" => x.GetCustomAttribute<HttpPOST>()?.MethodURI == methodURI
            });
            return method;
        }

        public void Dispose()
        {
            Stop();
        }
    }
}