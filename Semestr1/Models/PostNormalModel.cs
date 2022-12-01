namespace Semestr1.Models;

public class PostNormalModel
{
    public PostNormalModel(string name, UserModel user, AnimeModel anime)
    {
        Name = name;
        User = user;
        Anime = anime;
    }

    public PostNormalModel() {}
    public string Name { get; set; }
    public UserModel User{ get; set; }
    public AnimeModel Anime { get; set; }
}