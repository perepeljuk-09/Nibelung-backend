using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nibelung.Infrastructure.Configs
{
    public static class RefreshTokenConfig
    {
        public static TimeSpan LifeTime { get; } = TimeSpan.FromDays(1);
    }
}
