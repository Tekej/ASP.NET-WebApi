using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using static WebApplication1.PasswordCripter.SaltCreater;

namespace WebApplication1.PasswordCripter
{
    public class PasswordCripter
    {
        public static string passwordCripter(string? password,byte [] salt)
        {
            
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hashed;
        }
    }
}