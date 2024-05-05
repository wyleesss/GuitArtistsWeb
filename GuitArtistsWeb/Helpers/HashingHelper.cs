using System.Security.Cryptography;
using System.Text;

namespace GuitArtistsWeb.Helpers
{
    public static class HashingHelper
    {
        public static string GenerateSalt()
        {
            byte[] salt = new byte[16];
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return Convert.ToBase64String(salt);
        }

        public static string HashPassword(string password, string salt)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] saltedPassword = Encoding.UTF8.GetBytes(password + salt);
                byte[] hashedPassword = sha256.ComputeHash(saltedPassword);
                return Convert.ToBase64String(hashedPassword);
            }
        }

        public static string Hash(string obj)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] saltedPassword = Encoding.UTF8.GetBytes(obj);
                byte[] hashedPassword = sha256.ComputeHash(saltedPassword);
                return Convert.ToBase64String(hashedPassword);
            }
        }
    }
}
