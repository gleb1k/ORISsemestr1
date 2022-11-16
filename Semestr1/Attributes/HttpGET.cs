﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semestr1.Attributes
{
    public class HttpGET : Attribute
    {
        public string MethodURI { get; set; }
        public HttpGET(string methodURI)
        {
            MethodURI = methodURI;
        }
    }
}
