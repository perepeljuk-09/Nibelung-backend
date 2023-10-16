using Microsoft.IdentityModel.Tokens;
using Nibelung.Domain.Models;
using Nibelung.Infrastructure.Configs;
using Nibelung.Infrastructure.Helpers.Contract;
using Nibelung.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Nibelung.Infrastructure.Helpers
{
    public class JwtService : IJwtService
    {
        public string CreateToken(User user)
        {
            // generate token that is valid for 1 minute
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GetIdentity(new UserTokenData() { UserId = user.UserId}),
                Expires = DateTime.UtcNow + JwtTokenConfig.LifeTime,
                SigningCredentials = new SigningCredentials(JwtTokenConfig.SecurityKey, SecurityAlgorithms.HmacSha512)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal? GetTokenClaimsPrincipal(string token)
        {
            ClaimsPrincipal? claimsPrincipal = null;
            TokenValidationParameters parameters = new TokenValidationParameters()
            {
                RequireExpirationTime = false,
                RequireSignedTokens = false,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = JwtTokenConfig.SecurityKey
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                claimsPrincipal = tokenHandler.ValidateToken(token, parameters, out var validatedToken);
                return claimsPrincipal;
            }
            catch (Exception ex)
            {
                return claimsPrincipal;
            }
        }

        public bool ValidateToken(string token)
        {
            ClaimsPrincipal? claimsPrincipal = null;
            TokenValidationParameters parameters = new TokenValidationParameters()
            {
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = JwtTokenConfig.SecurityKey
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                claimsPrincipal = tokenHandler.ValidateToken(token, parameters, out var validatedToken);
                if(claimsPrincipal == null)
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        private ClaimsIdentity GetIdentity(UserTokenData user)
        {
            ClaimsIdentity identity = new ClaimsIdentity();
            identity.AddClaim(new Claim("user_id", JsonSerializer.Serialize(user)));
            return identity;
        }
    }
}
