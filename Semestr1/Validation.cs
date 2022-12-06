using System.Text.RegularExpressions;

namespace Semestr1;

public static class Validation
{
    public static bool CheckPassword(string password)
    {
        //минимум 4 символа, латинская буква(маленькая) и цифра
        string cond = @"^(?=.*[a-z])(?=.*[0-9])\S{4,50}$";

        return Regex.IsMatch(password, cond);
    }
    public static bool CheckLogin(string login)
    {
        
        string cond = @"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)";

        return Regex.IsMatch(login, cond);
    }

    public static bool CheckLoginAndPassword(string login, string password)
    {
        var temp1 = CheckLogin(login);
        var temp2 = CheckPassword(password);
        var result = temp1 && temp2;
        return result;
    }
    
}