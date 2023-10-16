using Microsoft.EntityFrameworkCore;
using Nibelung.Api.Mapper;
using Nibelung.Api.Models.Dto.Token;
using Nibelung.Api.Models.Dto.User;
using Nibelung.Api.Services.Contracts;
using Nibelung.Domain.Models;
using Nibelung.Infrastructure.Configs;
using Nibelung.Infrastructure.Db;
using Nibelung.Infrastructure.Extensions;
using Nibelung.Infrastructure.Helpers;
using Nibelung.Infrastructure.Helpers.Contract;
using Nibelung.Infrastructure.Models;
using System.Security.Claims;

namespace Nibelung.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly PgContext _db;
        private readonly IJwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthService(PgContext db, IJwtService jwtService, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> CreateUser(UserCreateDto dto)
        {

            User user = UserMapper.ToDomain(dto);

            PasswordHashAndSalt hashAndSalt = PasswordHelper.GetPasswordHashAndSalt(dto.Password);

            user.PasswordSalt = hashAndSalt.PasswordSalt;
            user.PasswordHash = hashAndSalt.PasswordHash;

            try
            {
                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"Error with create new user, => {ex}");
                return false;
            }
        }
        public async Task<TokensDto?> Authorization(UserAuthDto dto)
        {
            User? user = await _db.Users.FirstOrDefaultAsync(x => x.Email == dto.Login);

            if (user == null)
                return null;

            bool checkPass = PasswordHelper.CheckPassword(dto.Password, user.PasswordSalt, user.PasswordHash);

            if (!checkPass)
                return null;



            RefreshToken? lastToken = await _db.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == user.UserId);
            string refreshTokenString = RandomStringHelper.GetString();

            try
            {
                if (lastToken == null)
                {
                    RefreshToken refreshToken = new RefreshToken()
                    {
                        Token = refreshTokenString,
                        UserId = user.UserId,
                        ExpiredAt = DateTime.UtcNow + RefreshTokenConfig.LifeTime
                    };
                    await _db.RefreshTokens.AddAsync(refreshToken);
                }
                else
                {
                    lastToken.Token = refreshTokenString;
                    lastToken.ExpiredAt = DateTime.UtcNow + RefreshTokenConfig.LifeTime;
                    lastToken.AddedAt = DateTime.UtcNow;

                    _db.RefreshTokens.Update(lastToken);
                }

                await _db.SaveChangesAsync();
            }
            catch (Exception err)
            {
                await Console.Out.WriteLineAsync("Error with save refresh token" + err);
                return null;
            }

            TokensDto tokensDto = new TokensDto()
            {
                AccessToken = _jwtService.CreateToken(user),
                RefreshToken = refreshTokenString
            };

            return tokensDto;
        }

        public async Task<bool> Logout(string refreshToken)
        {
            RefreshToken? token = await _db.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);

            if (token == null)
                return false;

            UserTokenData? userTokenData = GetCurrentHttpContextUserTokenData();
            // unauthorize
            if (userTokenData == null || userTokenData.UserId != token.UserId)
                return false;

            _db.RefreshTokens.Remove(token);
            await _db.SaveChangesAsync();

            return true;
        }
        public async Task<TokensDto?> RefreshTokens(string refreshToken)
        {
            RefreshToken? token = await _db.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);

            if (token == null || token.ExpiredAt < DateTime.UtcNow)
                return null;

            UserTokenData? userTokenData = GetCurrentHttpContextUserTokenData();
            // unauthorize
            if (userTokenData == null || userTokenData.UserId != token.UserId)
                return null;

            User? user = await _db.Users.FirstOrDefaultAsync(x => x.UserId == token.UserId);

            if (user == null)
                return null;

            string newRefreshToken = RandomStringHelper.GetString();

            token.Token = newRefreshToken;
            token.ExpiredAt = DateTime.UtcNow + RefreshTokenConfig.LifeTime;
            token.AddedAt = DateTime.UtcNow;

            try
            {
                _db.RefreshTokens.Update(token);
                await _db.SaveChangesAsync();
            }
            catch (Exception err)
            {
                await Console.Out.WriteLineAsync("Was been error with update refresh token");
                return null;
            }

            TokensDto tokensDto = new TokensDto()
            {
                AccessToken = _jwtService.CreateToken(user),
                RefreshToken = newRefreshToken
            };

            return tokensDto;
        }
        public UserTokenData? GetCurrentHttpContextUserTokenData()
        {
            string? fulltoken = _httpContextAccessor?.HttpContext?.Request.Headers["Authorization"];
            string? token = fulltoken?.Split(' ').LastOrDefault();
            if (token == null)
                return null;

            ClaimsPrincipal? principal = _jwtService.GetTokenClaimsPrincipal(token);

            if (principal == null || principal.Claims == null)
                return null;

            UserTokenData? userTokenData = ClaimsExtensions.GetUserTokenData(principal.Claims);
            return userTokenData;
        }
    }
}
