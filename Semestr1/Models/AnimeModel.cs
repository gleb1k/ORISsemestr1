using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semestr1.Models
{
    public class AnimeModel : EntityBase
    {
        public string Name { get; set; }
        public string Author { get; set; }

        public string Description { get; set; }

        //УРЛ картинки
        public string ImageUrl { get; set; }

        public AnimeModel(int id, string name, string author, string description, string imageUrl)
        {
            Id = id;
            Name = name;
            Author = author;
            Description = description;
            ImageUrl = imageUrl;
        }

        public AnimeModel()
        {
        }
    }
}