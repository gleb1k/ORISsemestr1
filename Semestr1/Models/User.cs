using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semestr1.Models
{
    public class User : EntityBase
    {
        public string Login { get; }
        public string Password { get; set; }
        public int? Age { get; set; }
        public string? Mobile{ get; set; }
        public string? FavoriteAnimeName { get; set; }

        public User() { }

        public User(int id, string login, string password, int? age, string? mobile, string? favoriteAnime)
        {
            Id = id;
            Login = login;
            Password = password;
            Age = age;
            Mobile = mobile;
            FavoriteAnimeName = favoriteAnime;
        }
        ///TODO изменение пароля
        //public void ChangePassword(string oldPassword, string newPassword)
        //{
        //    if (Password != oldPassword)
        //        throw new Exception();

        //    Password = newPassword;
        //}
    }
}
