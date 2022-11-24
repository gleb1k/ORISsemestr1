using Microsoft.Data.SqlClient;
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
        public static User Login(string login, string password)
        {
            if (!CheckExistenceByLogin(login))
                return null;

            var myORM = new MyORM(connectionString);

            string query = $"SELECT * FROM Users where (Login='{login}' and Password='{password}')";
            myORM.ExecuteQuery<User>(query).FirstOrDefault();
            var temp = GetByLogin(login);
            return GetByLogin(login);
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
            return myORM.ExectureScalar<int>(nonQuery) > 0;
        }
        private static bool CheckExistenceById(int id)
        {
            var myORM = new MyORM(connectionString);
            string nonQuery = $"select count(*) from Users where Id='{id}'";
            return myORM.ExectureScalar<int>(nonQuery) > 0;
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
        public static User GetByLogin(string login)
        {
            if (!CheckExistenceByLogin(login))
                return null;

            var myORM = new MyORM(connectionString);
            string query = $"SELECT * FROM Users where Login='{login}'";

            return myORM.ExecuteQuery<User>(query).FirstOrDefault();
        }
        //todo
        public static User Update(User user)
        {
            //if (!CheckExistenceByLigin(user.Login))
            //    return false;

            //string nonQuery = $"UPDATE Users SET  " +
            //    $"FROM " +
            //    $"(SELECT * FROM Users WHERE Login='{user.Login}') AS Selected " +
            //    $"WHERE Users.Login = Selected.Login";

            //myORM.ExecuteNonQuery(nonQuery);

            //todo
            return null;
        }


    }
}
