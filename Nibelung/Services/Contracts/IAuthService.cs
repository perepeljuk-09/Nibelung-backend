using Nibelung.Api.Models.Dto.Token;
using Nibelung.Api.Models.Dto.User;
using Nibelung.Infrastructure.Models;

namespace Nibelung.Api.Services.Contracts
{
    public interface IAuthService
    {
        Task<bool> CreateUser(UserCreateDto dto);
        Task<TokensDto?> Authorization(UserAuthDto dto);
        Task<bool> Logout(string refreshToken);
        Task<TokensDto?> RefreshTokens(string refreshToken);
        UserTokenData? GetCurrentHttpContextUserTokenData();
    }
}
