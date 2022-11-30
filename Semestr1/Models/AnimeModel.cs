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

        public string Genre { get; set; }
        public string Studio { get; set; }
        public string AgeRating { get; set; }

        public AnimeModel()
        {
        }

        public AnimeModel(int id, string name, string author, string description, string imageUrl, string genre, string studio,
            string ageRating)
        {
            Id = id;
            Name = name;
            Author = author;
            Description = description;
            ImageUrl = imageUrl;
            Genre = genre;
            Studio = studio;
            AgeRating = ageRating;
        }
    }
}