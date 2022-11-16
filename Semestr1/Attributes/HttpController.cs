using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semestr1.Attributes
{
    public class HttpController : Attribute
    {
        public string ControllerName { get; set; }
        public HttpController(string controllerName)
        {
            ControllerName = controllerName;
        }
    }
}
