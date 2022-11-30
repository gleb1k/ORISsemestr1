using Semestr1.Models;
using Semestr1.Server;


namespace Semestr1.ORM
{
    public class UserDAO
    {
        private static readonly string ConnectionString = ServerSettings._connectionString;

        public static UserModel? Add(string login, string password)
        {
            if (CheckExistenceByLogin(login))
                return null;

            var myOrm = new MyORM(ConnectionString);
            string nonQuery = $"insert into Users (Login, Password) " +
                              $"values " +
                              $"(" +
                              $"'{login}'," +
                              $"'{password}'" +
                              $") RETURNING id;";
            var addedId = myOrm.ExecuteScalar<int>(nonQuery);
            GenerateUsername(addedId);
            
            return GetById(addedId);
        }

        public static void GenerateUsername(int userId)
        {
            var myOrm = new MyORM(ConnectionString);
            string nonQuery = $"Update Users " +
                              $"set username='user{userId}'" +
                              $"from (select * from Users where id = {userId}) as Selected " +
                              $"where users.id = Selected.id";
            myOrm.ExecuteNonQuery(nonQuery);
        }

        public static UserModel? Get(string login, string password)
        {
            if (!CheckExistenceByLogin(login))
                return null;

            var dbUser = GetByLogin(login);
            return dbUser?.Password == password ? dbUser : null;
        }


        public static UserModel? UpdateUser(int userId, string username, int age, string mobile)
        {
            if (!CheckExistenceById(userId))
                return null;

            string nonQuery = $"Update Users " +
                              $"Set age='{age}'," +
                              $"mobile='{mobile}', " +
                              $"username='{username}'" +
                              $"from (select * from Users where id = {userId}) as Selected " +
                              $"where users.id = Selected.id";

            var myOrm = new MyORM(ConnectionString);
            myOrm.ExecuteNonQuery(nonQuery);
            return GetById(userId);
        }

        public static bool Delete(UserModel userModel)
        {
            if (!CheckExistenceByLogin(userModel.Login))
                return false;

            var myOrm = new MyORM(ConnectionString);
            string nonQuery = $"DELETE FROM Users where Login='{userModel.Login}'";

            myOrm.ExecuteNonQuery(nonQuery);
            return true;
        }
        
        public static UserModel? GetById(int id)
        {
            if (!CheckExistenceById(id))
                return null;

            var myOrm = new MyORM(ConnectionString);
            string query = $"SELECT * FROM Users where Id='{id}'";

            return myOrm.ExecuteQuery<UserModel>(query).FirstOrDefault();
        }

        private static UserModel? GetByLogin(string login)
        {
            if (!CheckExistenceByLogin(login))
                return null;

            var myOrm = new MyORM(ConnectionString);
            string query = $"SELECT * FROM Users where Login='{login}'";

            return myOrm.ExecuteQuery<UserModel>(query).FirstOrDefault();
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
            var myOrm = new MyORM(ConnectionString);
            string nonQuery = $"select count(*) from Users where Login='{login}'";
            var temp = myOrm.CountRows(nonQuery);
            return temp > 0;
        }

        private static bool CheckExistenceById(int id)
        {
            var myOrm = new MyORM(ConnectionString);
            string nonQuery = $"select count(*) from Users where Id='{id}'";
            var temp = myOrm.CountRows(nonQuery);
            return temp > 0;
        }
    }
}