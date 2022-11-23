using Semestr1.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

                if (MethodHandler(context)) return;
                else
                {
                    ShowPage(context);
                }    

                //StaticFiles(context.Response, context.Request);
            }

        }
        private bool MethodHandler(HttpListenerContext _httpContext)
        {
            // объект запроса
            HttpListenerRequest request = _httpContext.Request;

            // объект ответа
            HttpListenerResponse response = _httpContext.Response;

            //пустой url
            if (_httpContext.Request.Url.Segments.Length < 2) return false;

            string controllerName = _httpContext.Request.Url.Segments[1].Replace("/", "");

            var assembly = Assembly.GetExecutingAssembly();

            // ищет контроллер accounts
            var controller = assembly.GetTypes().Where(t => Attribute.IsDefined(t, typeof(HttpController))).FirstOrDefault(c => c.Name.ToLower() == controllerName.ToLower());

            if (controller == null) return false;

            string[] strParams = _httpContext.Request.Url
                                    .Segments
                                    .Skip(2)
                                    .Select(s => s.Replace("/", ""))
                                    .ToArray();

            if (strParams.Length == 0) return false;

            var methods = controller.GetMethods().Where(t => t.GetCustomAttributes(true)
                                                              .Any(attr => attr.GetType().Name == $"Http{_httpContext.Request.HttpMethod}"));

            string methodURI = strParams[0];
            var method = methods.FirstOrDefault(x => _httpContext.Request.HttpMethod switch
            {
                "GET" => x.GetCustomAttribute<HttpGET>()?.MethodURI == methodURI,
                "POST" => x.GetCustomAttribute<HttpPOST>()?.MethodURI == methodURI
            });

            if (method == null) return false;

            object[] queryParams = null;

            var methodParams = method.GetParameters();
            

            if (_httpContext.Request.HttpMethod == HttpReqests.GET.ToString() )
            {
                //GET
                queryParams = strParams.Skip(1).Select(x => (object)x).ToArray();

                //меняю тип
                for (int i = 0; i < queryParams.Length; i++)
                {
                    queryParams[i] = Convert.ChangeType(queryParams[i], methodParams[i].ParameterType);
                }

                //тут чето делать

                var result = method.Invoke(Activator.CreateInstance(controller), queryParams);

                if (result != null && result is bool && (bool)result == false)
                {
                    
                    Pages.Pages.Show404(ref response);
                    Listening();
                    return true;
                }

                response.ContentType = "Application/json";

                byte[] buffer = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(result));
                response.ContentLength64 = buffer.Length;

                Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);

                output.Close();
            }
            else
            {
                //POST
                queryParams = GetLoginAndPassword(request);

                //меняю тип
                for (int i = 0; i < queryParams.Length; i++)
                {
                    queryParams[i] = Convert.ChangeType(queryParams[i], methodParams[i].ParameterType);
                }

                var result = method.Invoke(Activator.CreateInstance(controller), queryParams);
                if (result != null && result is bool && (bool)result == false)
                {
                    Pages.Pages.Show404(ref response);
                    Listening();
                    return true;
                }
                
                //TODO
                Pages.Pages.ShowHTMLPage(/*methodURI*/"profile",ref response);
                //response.StatusCode = 201;
                //response.ContentEncoding = Encoding.UTF8;

                //string message = $"<h1>201<h1> <h2>{queryParams[0]} {queryParams[1]}<h2>";
                //var buffer = Encoding.UTF8.GetBytes(message);

                //Stream output = response.OutputStream;
                //output.Write(buffer, 0, buffer.Length);

                //output.Close();
            }

            Listening();

            return true;
        }

        //прием данных с полей логин и пароль (ретурнуть словарь) Колхоз
        public string[] GetLoginAndPassword(HttpListenerRequest request)
        {
            if (!request.HasEntityBody)
            {
                Console.WriteLine("No client data was sent with the request.");
                return null;
            }
            Stream body = request.InputStream;
            Encoding encoding = request.ContentEncoding;

            StreamReader reader = new StreamReader(body, encoding);
            string s = reader.ReadToEnd();

            body.Close();
            reader.Close();

            var charLogin = s.ToCharArray().Skip(6).TakeWhile(item => item != '&').ToArray();
            string login = new string(charLogin).Replace("%40", "@");

            var charPassword = s.SkipWhile(item => item != '&').Skip(10).ToArray();
            string password = new string(charPassword);

            string[] strParams = new string[] { login, password };

            var dict = new Dictionary<string, string>
            {
                { "Login", login},
                { "Password", password},
            };
            //TODO

            return strParams;

        }
        private void Show404(ref HttpListenerResponse response)
        {
            response.Headers.Set("Content-Type", "text/html");
            response.StatusCode = 404;
            response.ContentEncoding = Encoding.UTF8;
            string err = "<h1>404<h1> <h2>The resource can not be found.<h2>";
            byte[] buffer = Encoding.UTF8.GetBytes(err);
            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            //закрываем поток
            output.Close();
        }
        private void ShowPage(HttpListenerContext _httpContext)
        {
            var response = _httpContext.Response;
            try
            {
                response.Headers.Set("Content-Type", "text/html");
                string[] strParams = _httpContext.Request.Url
                                        .Segments
                                        .Skip(2)
                                        .Select(s => s.Replace("/", ""))
                                        .ToArray();
                string directory = strParams[0];
                if (!Pages.Pages.ShowHTMLPage(directory, ref response))

                    Show404(ref response);
            }
            catch
            {
                Show404(ref response);
                Listening();
            }
            Listening();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
