using Nibelung.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Nibelung.Infrastructure.Helpers.Contract
{
    public interface IJwtService
    {
        string CreateToken(User user);
        bool ValidateToken(string token);
        ClaimsPrincipal? GetTokenClaimsPrincipal(string token);
    }
}
