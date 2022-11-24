using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semestr1.Models
{
    public class User : EntityBase
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public int? Age { get; set; }
        public string Mobile { get; set; }
        public int? FavoriteAnimeId { get; set; }

        public User() { }

        public User(int id, string login, string password, int? age, string mobile, int? favoriteAnime)
        {
            Id = id;
            Login = login;
            Password = password;
            Age = age;
            Mobile = mobile;
            FavoriteAnimeId = favoriteAnime;
        }
    }
}
