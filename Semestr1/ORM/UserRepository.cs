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
            string nonQuery = $"select count(*) from Users where Login='{login}'";
            return myORM.ExectureScalar<int>(nonQuery) > 0;
        }
        public bool Delete(User user)
        {
            if (!CheckExistenceByLigin(user.Login))
                return false;

            var myORM = new MyORM(connectionString);
            string nonQuery = $"DELETE FROM Users where Login='{user.Login}'";

            myORM.ExecuteNonQuery(nonQuery);
            return true;
        }
        //TODO
        public User GetById(int id)
        {
            throw new NotImplementedException();
        }
        public bool Update(User user)
        {
            if (!CheckExistenceByLigin(user.Login))
                return false;

            var myORM = new MyORM(connectionString);

            Type t = typeof(User);
            var args = t.GetProperties();

            var values = args.Select(value => $"@{value.GetValue(user)}").ToArray();

            var argsWithDog = args.Select(value => $"@{value.ToString().Split(" ")[1]}").ToArray();
            var argsWithoutDog = args.Select(x => x.Name).ToArray();

            foreach (var parameter in args)
            {
                var sqlParameter = new SqlParameter($"@{parameter.Name}", parameter.GetValue(user));
                myORM._cmd.Parameters.Add(sqlParameter);
            }
            //todo ЕСЛИ В НЕ ИЗМЕНИЛ БД (ТОЕСТЬ ID ЕЩЕ ЕСТЬ, ТО РАБОТАТЬ НЕ БУДЕТ)
            //Row = @Value. without ID
            string setQuery = "";
            for (int i = 0; i < args.Length; i++)
            {
                setQuery += $"{argsWithoutDog[i]}={argsWithDog[i]}, ";
            }
            setQuery = setQuery.Remove(setQuery.Length - 2, 2);

            string nonQuery = $"UPDATE Users SET {setQuery} " +
                $"FROM " +
                $"(SELECT * FROM Users WHERE Login='{user.Login}') AS Selected " +
                $"WHERE Users.Login = Selected.Login";

            myORM.ExecuteNonQuery(nonQuery);
            return true;
        }


    }
}
