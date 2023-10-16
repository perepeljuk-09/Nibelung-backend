using Nibelung.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Nibelung.Infrastructure.Extensions
{
    public static class ClaimsExtensions
    {
        public static UserTokenData? GetUserTokenData(this IEnumerable<Claim> claims)
        {
            var userClaim = claims?.FirstOrDefault(x => x.Type == "user_id")?.Value;
            if (userClaim == null)
                return null;

            UserTokenData? userTokenData = JsonSerializer.Deserialize<UserTokenData>(userClaim);

            return userTokenData;
        }
    }
}
