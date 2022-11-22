using Semestr1.Models;
using Semestr1.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semestr1.Attributes;
using Semestr1.Server;

namespace Semestr1.Contollers
{
    [HttpController("users")]
    public class Users
    {
        [HttpGET("getuser")]
        public User GetUser(int id)
        {
            var rep = new Repository<User>();
            return rep.GetById(id);
        }

        [HttpGET("getusers")]
        public List<User> GetUsers()
        {
            var myORM = new MyORM(ServerSettings._connectionString);
            return myORM.Select<User>().ToList();
        }

        [HttpPOST("registerPOST")]
        public static void Register(string login, string password)
        {
            var myORM = new MyORM(ServerSettings._connectionString);
        }

        [HttpPOST("loginPOST")]
        public static bool Login(string login, string password)
        {
            var myORM = new MyORM(ServerSettings._connectionString);
            int count = 0;
            count += myORM.AddParameter("@login", login).AddParameter("@password", password)
                .ExecuteNonQuery("select * from [dbo].[Table] where Login='@login' and Password='@password'");
            if (count > 0)
                return true;
            else
                return false;
        }

    }
}
