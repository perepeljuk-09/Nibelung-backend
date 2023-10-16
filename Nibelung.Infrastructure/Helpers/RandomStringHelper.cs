using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Nibelung.Infrastructure.Helpers
{
    public static class RandomStringHelper
    {
        public static string GetString()
        {
            RNGCryptoServiceProvider cryptoService = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[64];
            cryptoService.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
