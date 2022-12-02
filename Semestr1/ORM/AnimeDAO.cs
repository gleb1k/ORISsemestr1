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

        public static AnimeModel? Add(string name, string description,  string genre, string studio, string ageRating, string imageUrl = "not defined")
        {
            //если такое уже есть то добавить нельзя
            if (CheckExistenceByName(name))
                return null;
            
            var myOrm = new MyORM(ConnectionString);

            myOrm.AddParameter($"@name",name);
            myOrm.AddParameter($"@description",description);
            myOrm.AddParameter($"@genre",genre);
            myOrm.AddParameter($"@studio",studio);
            myOrm.AddParameter($"@ageRating",ageRating);
            myOrm.AddParameter($"@imageUrl",imageUrl);
            

            string nonQuery = $"insert into Animes (Name,description,genre,studio, agerating,imageurl) " +
                              $"VALUES " +
                              $"(" +
                              $"'{@name}'," +
                              $"'{@description}'," +
                              $"'{@genre}'," +
                              $"'{@studio}'," +
                              $"'{@ageRating}'," +
                              $"'{@imageUrl}'" +
                              $") RETURNING id;";
            
            var addedId = myOrm.ExecuteScalar<int>(nonQuery);
            var anime = GetById(addedId);

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