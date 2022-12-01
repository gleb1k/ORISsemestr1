using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semestr1.ORM;

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
        public UserNormalModel GetNormalModel()
        {
            if (FavoriteAnimeId == null)
            {
                return new UserNormalModel(Login,Password,Username,Age,Mobile,AvatarUrl,null);
            }
            var id = Convert.ToInt32(FavoriteAnimeId);
            var anime = AnimeDAO.GetById(id);
            return new UserNormalModel(Login,Password,Username,Age,Mobile,AvatarUrl,anime);

        }
    }
}