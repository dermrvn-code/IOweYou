using System.Security.Cryptography;
using System.Text;

namespace IOweYou.Helper;

public class Hasher
{
    public static string UrlSecureHashValue(string value)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] valueBytes = Encoding.UTF8.GetBytes(value);
            byte[] hashBytes = sha256.ComputeHash(valueBytes);
            
            string base64Hash = Convert.ToBase64String(hashBytes);
            string urlSafeBase64 = base64Hash.Replace('+', '-').Replace('/', '_').TrimEnd('=');
            return urlSafeBase64;
        }
    }
}