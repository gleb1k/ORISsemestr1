using Semestr1.ORM;

namespace Semestr1.Models;

public class PostModel : EntityBase
{
    public PostModel(string name, int userId, int animeId)
    {
        Name = name;
        UserId = userId;
        AnimeId = animeId;
    }
    public PostModel() {}
    public string Name { get; set; }
    public int UserId { get; set; }
    public int AnimeId { get; set; }

    public PostNormalModel GetNormalModel()
    {
        var user = UserDAO.GetById(UserId);
        var anime = AnimeDAO.GetById(AnimeId);

        return new PostNormalModel(Name, user, anime);
    }
}