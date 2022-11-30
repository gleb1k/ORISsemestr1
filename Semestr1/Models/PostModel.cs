namespace Semestr1.Models;

public class PostModel : EntityBase
{
    public PostModel(string name, int postAuthorId, int animeId)
    {
        Name = name;
        PostAuthorId = postAuthorId;
        AnimeId = animeId;
    }

    public string Name { get; set; }
    public int PostAuthorId { get; set; }
    public int AnimeId { get; set; }
}