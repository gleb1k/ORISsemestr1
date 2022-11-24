using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semestr1.Attributes
{
    public class HttpPOST : Attribute
    {
        public string MethodURI { get; set; }

        public HttpPOST(string methodURI)
        {
            MethodURI = methodURI;
        }
    }
}