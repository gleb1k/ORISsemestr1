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
        private static readonly string connectionString = ServerSettings._connectionString;

        public static bool AddAnime(Anime anime)
        {
            if (!CheckExistenceById(anime.Id))
                return false;

            var myORM = new MyORM(connectionString);
            string nonQuery = $"insert into Animes (Id,Name,Author, Description) " +
                              $"VALUES " +
                              $"(" +
                              $"'{anime.Id}'," +
                              $"'{anime.Name},'" +
                              $"'{anime.Author}," +
                              $"'{anime.Description}" +
                              $")";
            myORM.ExecuteNonQuery(nonQuery);

            return true;
        }

        public static bool Delete(Anime anime)
        {
            if (!CheckExistenceById(anime.Id))
                return false;

            var myORM = new MyORM(connectionString);
            string nonQuery = $"DELETE FROM Animes where Id='{anime.Id}'";

            myORM.ExecuteNonQuery(nonQuery);
            return true;
        }

        private static bool CheckExistenceById(int id)
        {
            var myORM = new MyORM(connectionString);
            string nonQuery = $"select count(*) from Animes where Id='{id}'";
            return myORM.ExectureScalar<int>(nonQuery) > 0;
        }
    }
}