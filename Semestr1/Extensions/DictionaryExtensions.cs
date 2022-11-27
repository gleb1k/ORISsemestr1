namespace Semestr1.Extensions;

public static class DictionaryExtensions
{
    /// <summary>
    /// Проверяет чтобы не было пустых строк в словаре
    /// </summary>
    /// <param name="dict"></param>
    /// <returns>false -> пустые строки есть
    /// true -> пустых строк нет</returns>
    public static bool CheckEmptyness(this Dictionary<string, string> dict)
    {
        foreach (var item in dict)
        {
            if (item.Value == "")
                return false;
        }

        return true;
    }
}