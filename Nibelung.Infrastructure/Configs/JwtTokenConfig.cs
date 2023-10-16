using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nibelung.Infrastructure.Configs
{
    public static class JwtTokenConfig
    {
        public static TimeSpan LifeTime { get; } = TimeSpan.FromMinutes(5);
        public static string? SIGN_KEY { get; } = "krc_pon_vsja_budet_nibe";
        public static SymmetricSecurityKey SecurityKey { get; } = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SIGN_KEY));
    }
}
