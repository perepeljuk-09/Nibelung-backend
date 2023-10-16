using Microsoft.AspNetCore.Http;
using Nibelung.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nibelung.Infrastructure.Models
{
    public interface IUserContext
    {
        UserTokenData? User { get; }
    }

    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public UserTokenData? User { get; }
        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            User = httpContextAccessor?.HttpContext?.User?.Claims.GetUserTokenData();
        }

    }
}
