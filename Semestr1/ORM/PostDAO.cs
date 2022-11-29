using Semestr1.Models;
using Semestr1.Server;

namespace Semestr1.ORM;

public static class PostDAO
{
    private static readonly string ConnectionString = ServerSettings._connectionString;

    public static PostModel Add(int userId, int animeId)
    {
        //если такое уже есть то добавить нельзя
        if (CheckExistence(userId, animeId))
            return null;

        var myOrm = new MyORM(ConnectionString);
        string nonQuery = $"insert into Posts (authorid, animeid) " +
                          $"VALUES " +
                          $"(" +
                          $"'{userId}'," +
                          $"'{animeId}'" +
                          $") RETURNING id;";

        var addedId = myOrm.ExecuteScalar<int>(nonQuery);

        var post = GetById(addedId);
        return post;
    }

    public static PostModel GetById(int id)
    {
        var myOrm = new MyORM(ConnectionString);
        string query = $"SELECT * FROM posts where Id='{id}'";

        return myOrm.ExecuteQuery<PostModel>(query).FirstOrDefault();
    }

    public static bool CheckExistence(int authorId, int animeId)
    {
        var myOrm = new MyORM(ConnectionString);
        string nonQuery = $"select count(*) from Posts where authorId='{authorId}' and animeId='{animeId}'";
        var temp = myOrm.CountRows(nonQuery);
        return temp > 0;
    }
}