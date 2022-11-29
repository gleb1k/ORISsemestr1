namespace Semestr1.Models;

public class PostModel : EntityBase
{
    public PostModel(int postAuthorId, int animeId)
    {
        PostAuthorId = postAuthorId;
        AnimeId = animeId;
    }

    public int PostAuthorId { get; set; }
    public int AnimeId { get; set; }
}