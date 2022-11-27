using System.Web;
using Semestr1.Models;
using Scriban;

namespace Semestr1;

public static class ScribanMethods
{
    private const string PublicFolder = "site";
    private static readonly string PublicFolderPath = Path.Join(Directory.GetCurrentDirectory(), PublicFolder);

    public static async Task GenerateProfilePage(UserModel userModel)
    {
        var templatePath = Path.Join(PublicFolderPath, @"\templates\profile.html");
        var resultPath = Path.Join(PublicFolderPath, @"\profile\profile.html");
        if (File.Exists(templatePath))
        {
            string html = await File.ReadAllTextAsync(templatePath);

            // Parse a scriban template
            var template = Template.Parse(html);
            var result = template.RenderAsync(userModel);

            await File.WriteAllTextAsync(resultPath, result.Result);
        }
        else
        {
            await File.WriteAllTextAsync(resultPath, "File doesn't found");
        }
    }

    public static async Task GenerateHomePage(List<AnimeModel> animeList)
    {
        var templatePath = Path.Join(PublicFolderPath, @"\templates\home.html");
        var resultPath = Path.Join(PublicFolderPath, @"\home\home.html");
        if (File.Exists(templatePath))
        {
            string html = await File.ReadAllTextAsync(templatePath);

            // Parse a scriban template
            var template = Template.Parse(html);
            var result = template.RenderAsync(new { Animes = animeList });

            await File.WriteAllTextAsync(resultPath, result.Result);
        }
        else
        {
            await File.WriteAllTextAsync(resultPath, "File doesn't found");
        }
    }
}