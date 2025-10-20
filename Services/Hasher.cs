using System.Security.Cryptography;
using System.Text;

namespace BookShop.Services;

public class Hasher
{
    public string GetHash(string text)
    {
        var bytes = Encoding.Unicode.GetBytes(text);
        var sha = new SHA256Managed();
        var hash = sha.ComputeHash(bytes);
        var hashString = string.Empty;
        
        foreach (var x in hash)
        {
            hashString += $"{x:x2}";
        }
        
        return hashString;
    }
}