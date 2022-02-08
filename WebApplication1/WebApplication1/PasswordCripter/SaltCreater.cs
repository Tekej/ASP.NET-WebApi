using System;
using System.Security.Cryptography;

namespace WebApplication1.PasswordCripter
{
    public class SaltCreater
    {
        public static byte[] saltCreater()
        {
            var salt = new byte[128 / 8];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return salt;
        }
    }
}