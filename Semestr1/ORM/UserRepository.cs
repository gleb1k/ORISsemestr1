using Microsoft.Data.SqlClient;
using Semestr1.Models;
using Semestr1.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semestr1.ORM
{
    public class UserRepository : IUserRepository
    {
        private readonly string connectionString = ServerSettings._connectionString;
        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
        /// false -> user already registered
        /// true -> registration was succesfull
        /// </returns>
        public bool Register(User user)
        {
            if (CheckExistenceByLigin(user.Login))
                return false;

            var myORM = new MyORM(connectionString);
            string nonQuery = $"insert into Users (Login, Password) " +
                $"values ('{user.Login}','{user.Password}')";
            myORM.ExecuteNonQuery(nonQuery);

            return true;
        }
        /// <summary>
        /// authorization by login and password
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
        /// false -> user has not registered or incorrect values (login or passw)
        /// true -> authorization was succesfull
        /// </returns>
        public bool Login(User user)
        {
            if (!CheckExistenceByLigin(user.Login))
                return false;

            var myORM = new MyORM(connectionString);
            var listOfUsers = myORM.Select<User>().ToList();
            User foundedUser = null;
            foreach (var userItem in listOfUsers)
            {
                if (userItem.Login == user.Login && userItem.Password == user.Password)
                {
                    foundedUser = userItem;
                    break;
                }
            }
            if (foundedUser == null)
                return false;
            else
                return true;
        }
        /// <summary>
        /// </summary>
        /// <param name="login"></param>
        /// <returns>
        /// true -> exist
        /// false -> doesn't exist
        /// </returns>
        private bool CheckExistenceByLigin(string login)
        {
            var myORM = new MyORM(connectionString);
            string nonQuery = $"select * from Users where Login='{login}')";
            return myORM.ExecuteNonQuery(nonQuery) > 0;
        }
        /// <summary>
        /// Registration user by login and password
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        public void Register(string login, string password)
        {
            var myORM = new MyORM(connectionString);
            string nonQuery = $"insert into Users (Login, Password) " +
                $"values ({login},{password})";
            myORM.ExecuteNonQuery(nonQuery);
        }

        public void Delete(User user)
        {
            var myORM = new MyORM(connectionString);
        }

        public User GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(User user)
        {
            throw new NotImplementedException();
        }


    }
}
