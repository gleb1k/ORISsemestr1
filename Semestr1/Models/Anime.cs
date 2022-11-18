using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semestr1.Models
{
    public class Anime : EntityBase
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public Anime(int id, string name, string author, string description, string url)
        {
            Id = id;
            Name = name;
            Author = author;
            Description = description;
            Url = url;
        }
        public Anime() { }
    }
}
