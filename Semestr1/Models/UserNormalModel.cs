namespace Semestr1.Models;

public class UserNormalModel
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string Username { get; set; }
    public int? Age { get; set; }
    public string Mobile { get; set; }
    public string AvatarUrl { get; set; }

    public AnimeModel Anime { get; set; }

    public UserNormalModel()
    {
    }

    public UserNormalModel(string login, string password, string username, int? age, string mobile,
        string avatarUrl, AnimeModel anime)
    {
        Login = login;
        Password = password;
        Username = username;
        Age = age;
        Mobile = mobile;
        AvatarUrl = avatarUrl;
        Anime = anime;
    }
}