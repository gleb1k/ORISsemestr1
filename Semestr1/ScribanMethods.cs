using System.Web;
using Semestr1.Models;
using Scriban;
namespace Semestr1;

public static class ScribanMethods
{
    private const string PublicFolder = "site";
    private static readonly string PublicFolderPath = Path.Join(Directory.GetCurrentDirectory(), PublicFolder);
    
    public static async Task GenerateProfilePage(string path, UserModel userModel)
    {
        var fullPath = Path.Join(PublicFolderPath, path);
        if (File.Exists(fullPath))
        {
            string html = await File.ReadAllTextAsync(fullPath);

            // Parse a scriban template
            var template = Template.Parse(html);
            var result = template.RenderAsync(userModel); 
            
            await File.WriteAllTextAsync(fullPath, result.Result);
        }
        else
        {
            await File.WriteAllTextAsync(fullPath, "File doesn't found");
        }
    }
    public static async Task GenerateHomePage(string path, List<AnimeModel> animeList)
    {
        var fullPath = Path.Join(PublicFolderPath, path);
        if (File.Exists(fullPath))
        {
            string html = await File.ReadAllTextAsync(fullPath);

            // Parse a scriban template
            var template = Template.Parse(html);
            var result = template.RenderAsync(new { Animes = animeList }); 
            
            await File.WriteAllTextAsync(fullPath, result.Result);
        }
        else
        {
            await File.WriteAllTextAsync(fullPath, "File doesn't found");
        }
    }
}