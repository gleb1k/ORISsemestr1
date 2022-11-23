using Semestr1.Models;
using Semestr1.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semestr1.Attributes;
using Semestr1.Server;
using System.Security.Cryptography.X509Certificates;

namespace Semestr1.Contollers
{
    [HttpController("users")]
    public class Users
    {
        private UserRepository rep = new UserRepository() ;
        [HttpGET("getuser")]
        public User GetUser(int id)
        {
            return rep.GetById(id);
        }

        //[HttpGET("getusers")]
        //public List<User> GetUsers()
        //{
        //    return rep.GetAll();
        //}
        [HttpPOST("updatePOST")]
        public bool? Update(string age, string mobile, string favoriteAnime)
        {
            string login = "";
            string password = "";
            var user = new User(login, password, age, mobile, favoriteAnime);
            throw new NotImplementedException();
        }

        [HttpPOST("registerPOST")]
        public bool? Register(string login, string password)
        {
            var newUser = new User(login, password, null, null, null);
            return rep.Register(newUser);     
        }

        [HttpPOST("loginPOST")]
        public bool? Login(string login, string password)
        {
            var newUser = new User(login, password, null, null, null);
            return rep.Login(newUser);
            //todo вошел в систему
        }

    }
}
