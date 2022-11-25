using Semestr1.Contollers;
using Semestr1.Models;
using Semestr1.Server;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semestr1.ORM
{
    public class UserDAO
    {
        private static readonly string connectionString = ServerSettings._connectionString;

        public static User AddUser(string login, string password)
        {
            if (CheckExistenceByLogin(login))
                return null;

            var myORM = new MyORM(connectionString);
            string nonQuery = $"insert into Users (Login, Password) " +
                              $"values ('{login}','{password}')";
            myORM.ExecuteNonQuery(nonQuery);

            return GetByLogin(login);
        }

        public static User GetUser(string login, string password)
        {
            if (!CheckExistenceByLogin(login))
                return null;

            var dbUser = GetByLogin(login);
            return dbUser.Password == password ? dbUser : null;
        }

        //todo
        public static User UpdateUser(int userId, int age, string mobile)
        {
            if (!CheckExistenceById(userId))
                return null;

            string nonQuery = $"Update Users " +
                              $"Set age='{age}'," +
                              $"mobile='{mobile}' " +
                              $"from (select * from Users where id = {userId}) as Selected " +
                              $"where users.id = Selected.id";

            var myORM = new MyORM(connectionString);
            myORM.ExecuteNonQuery(nonQuery);
            return GetById(userId);
        }

        public static bool Delete(User user)
        {
            if (!CheckExistenceByLogin(user.Login))
                return false;

            var myORM = new MyORM(connectionString);
            string nonQuery = $"DELETE FROM Users where Login='{user.Login}'";

            myORM.ExecuteNonQuery(nonQuery);
            return true;
        }

        public static User GetById(int id)
        {
            if (!CheckExistenceById(id))
                return null;

            var myORM = new MyORM(connectionString);
            string query = $"SELECT * FROM Users where Id='{id}'";

            return myORM.ExecuteQuery<User>(query).FirstOrDefault();
        }

        private static User GetByLogin(string login)
        {
            if (!CheckExistenceByLogin(login))
                return null;

            var myORM = new MyORM(connectionString);
            string query = $"SELECT * FROM Users where Login='{login}'";

            return myORM.ExecuteQuery<User>(query).FirstOrDefault();
        }

        /// <summary>
        /// </summary>
        /// <param name="login"></param>
        /// <returns>
        /// true -> exist
        /// false -> doesn't exist
        /// </returns>
        private static bool CheckExistenceByLogin(string login)
        {
            var myORM = new MyORM(connectionString);
            string nonQuery = $"select count(*) from Users where Login='{login}'";
            var temp = myORM.CountRows(nonQuery);
            return temp > 0;
        }

        private static bool CheckExistenceById(int id)
        {
            var myORM = new MyORM(connectionString);
            string nonQuery = $"select count(*) from Users where Id='{id}'";
            var temp = myORM.CountRows(nonQuery);
            return temp > 0;
        }
    }
}