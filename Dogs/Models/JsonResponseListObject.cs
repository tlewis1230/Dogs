using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dogs.Models
{
    public class JsonResponseListObject
    {
        public string status { get; set; }
        public string[] message { get; set; }
    }
}
