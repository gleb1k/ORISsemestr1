using Semestr1.Models;
using Semestr1.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semestr1.ORM
{
    public class AnimeDAO
    {
        private static readonly string ConnectionString = ServerSettings._connectionString;

        public static AnimeModel? Add(string name, string author, string description)
        {
            //если такое уже есть то добавить нельзя
            if (CheckExistenceByName(name))
                return null;
            
            var myOrm = new MyORM(ConnectionString);
            string nonQuery = $"insert into Animes (Name,Author, Description) " +
                              $"VALUES " +
                              $"(" +
                              $"'{name}'," +
                              $"'{author}'," +
                              $"'{description}'" +
                              $")";
            myOrm.ExecuteNonQuery(nonQuery);

            var anime = GetByName(name);
            return anime;
        }

        public static AnimeModel? GetById(int id)
        {
            if (!CheckExistenceById(id))
                return null;

            var myOrm = new MyORM(ConnectionString);
            string query = $"SELECT * FROM Animes where Id='{id}'";

            return myOrm.ExecuteQuery<AnimeModel>(query).FirstOrDefault();
        }

        public static AnimeModel? GetByName(string name)
        {
            var myOrm = new MyORM(ConnectionString);
            string query = $"SELECT * FROM Animes where Name='{name}'";

            return myOrm.ExecuteQuery<AnimeModel>(query).FirstOrDefault();
        }

        public static List<AnimeModel> GetAll()
        {
            var myOrm = new MyORM(ConnectionString);
            string query = $"SELECT * FROM Animes";
            return myOrm.ExecuteQuery<AnimeModel>(query).ToList();
        }

        public static bool Delete(AnimeModel animeModel)
        {
            if (!CheckExistenceById(animeModel.Id))
                return false;

            var myOrm = new MyORM(ConnectionString);
            string nonQuery = $"DELETE FROM Animes where Id='{animeModel.Id}'";

            myOrm.ExecuteNonQuery(nonQuery);
            return true;
        }

        private static bool CheckExistenceById(int id)
        {
            var myOrm = new MyORM(ConnectionString);
            string nonQuery = $"select count(*) from Animes where Id='{id}'";
            var temp = myOrm.CountRows(nonQuery);
            return temp > 0;
        }
        private static bool CheckExistenceByName(string name)
        {
            var myOrm = new MyORM(ConnectionString);
            string nonQuery = $"select count(*) from Animes where Name='{name}'";
            var temp = myOrm.CountRows(nonQuery);
            return temp > 0;
        }
    }
}