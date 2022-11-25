using System.Web;
using Semestr1.Models;
using Scriban;
namespace Semestr1;

public static class ScribanMethods
{
    private const string PublicFolder = "site";
    private static readonly string PublicFolderPath = Path.Join(Directory.GetCurrentDirectory(), PublicFolder);
    
    //var encoded = HttpUtility.HtmlEncode(unencoded);
    public static string GenerateProfile(string path, User user)
    {
        var fullPath = Path.Join(PublicFolderPath, path);
        if (File.Exists(fullPath))
        {
            string html = File.ReadAllText(fullPath);

            // Parse a scriban template
            var template = Template.Parse(html);
            var result = template.Render(user); 
            
            var pathToHtml = Path.Join(PublicFolderPath, "/profile/profile.html");
            File.WriteAllText(pathToHtml, result);
            return result;
        }
        else
        {
            return "File doesn't found";
        }
    }
}