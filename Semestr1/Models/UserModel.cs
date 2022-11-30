using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semestr1.Models
{
    public class UserModel : EntityBase
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public int? Age { get; set; }
        public string Mobile { get; set; }
        public string AvatarUrl { get; set; }

        public int? FavoriteAnimeId { get; set; }


        public UserModel()
        {
        }

        public UserModel(int id, string login, string password, string username, int? age, string mobile,
            string avatarUrl, int? favoriteAnimeId)
        {
            Id = id;
            Login = login;
            Password = password;
            Username = username;
            Age = age;
            Mobile = mobile;
            AvatarUrl = avatarUrl;
            FavoriteAnimeId = favoriteAnimeId;
        }
    }
}