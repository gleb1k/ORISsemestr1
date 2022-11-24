using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Semestr1.Attributes;
using Semestr1.Extensions;

namespace Semestr1.Contollers
{
    [HttpController("anime")]
    public class Anime
    {
        [HttpGET("home")]
        public static async Task ShowIndex(HttpListenerContext context)
        {
            await context.ServerPage(@"\home\home.html");
        }

        [HttpGET("register")] 
        public static async Task ShowRegister(HttpListenerContext context)
        {
            await context.ServerPage(@"\register\register.html");
        }

        [HttpGET("login")] 
        public static async Task ShowLogin(HttpListenerContext context)
        {
            await context.ServerPage(@"\login\login.html");
        }
        [HttpGET("profile")]
        public static async Task ShowProfile(HttpListenerContext context)
        {
            await context.ServerPage(@"\profile\profile.html");
        }
    }
}
