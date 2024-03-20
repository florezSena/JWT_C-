using System.Security.Cryptography;
using System.Text;

namespace Lemon.Models
{
    public class PasswordEncoder
    {
        public static string EncodePassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            string encodedPassword = EncodePassword(password);
            return encodedPassword == hashedPassword;
        }
    }
}
