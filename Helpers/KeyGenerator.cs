using System.Security.Cryptography;

namespace Login_new.Helpers
{
    public class KeyGenerator
    {
        public static void Main()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] key = new byte[32]; // 256-bit key
                rng.GetBytes(key);
                string base64Key = Convert.ToBase64String(key);
                Console.WriteLine(base64Key);
            }
        }
    }
}
