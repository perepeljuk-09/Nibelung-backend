using Nibelung.Infrastructure.Configs;
using Nibelung.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Nibelung.Infrastructure.Helpers
{
    public static class PasswordHelper
    {
        public static PasswordHashAndSalt GetPasswordHashAndSalt(string password)
        {
            using var hmac = new HMACSHA512();
            PasswordHashAndSalt result = new PasswordHashAndSalt();

            result.PasswordSalt = Convert.ToBase64String(hmac.Key);
            result.PasswordHash = CreatePasswordHash(password, result.PasswordSalt, hmac);

            return result;
        }

        public static string CreatePasswordHash(string password, string passwordSalt, HMAC algoritm)
        {
            return Convert.ToBase64String(

                algoritm.ComputeHash(

                    Encoding.UTF8.GetBytes(
                        
                            Convert.ToBase64String(algoritm.ComputeHash(Encoding.UTF8.GetBytes(passwordSalt + password))) +
                            Convert.ToBase64String(algoritm.ComputeHash(Encoding.UTF8.GetBytes(PasswordConfig.Salt + password)))
                        )
                    )
                );
        }
        public static bool CheckPassword(string password, string passwordSalt, string passwordHash)
        {
            using var hmac = new HMACSHA512(Convert.FromBase64String(passwordSalt));

            string hash = CreatePasswordHash(password, passwordSalt, hmac);

            return passwordHash == hash;
        }
    }
}
