using Microsoft.EntityFrameworkCore;
using Nibelung.Api.Mapper;
using Nibelung.Api.Models.Dto.User;
using Nibelung.Api.Services.Contracts;
using Nibelung.Domain.Models;
using Nibelung.Infrastructure.Db;
using Nibelung.Infrastructure.Models;

namespace Nibelung.Api.Services
{
    public class UserService : IUserService
    {
        private readonly PgContext _db;
        private readonly IUserContext _userContext;
        public UserService(PgContext db, IUserContext userContext)
        {
            _db = db;
            _userContext = userContext;
        }

        public async Task<UserDto> GetUserById(int userId)
        {

            int? consumerId = _userContext.User?.UserId;

            if (consumerId == null) throw new Exception("Нет айди пользователя");


            User? user = await _db.Users
                .FirstOrDefaultAsync(x => x.UserId == userId);


            if (user == null)
                throw new Exception("Пользователь не найден");

            UserDto userDto = UserMapper.ToUserDto(user);

            return userDto;
        }

        public async Task<UserDto> UpdateUser(int id,UserUpdateDto dto)
        {
            UserTokenData? userTokenData = _userContext.User;

            // check request from User or not
            if (userTokenData != null && userTokenData.UserId != id)
                throw new Exception("Нет доступа");

            User? user = await _db.Users.FirstOrDefaultAsync(x => x.UserId == id);

            if (user == null)
                throw new Exception("Пользователь не найден");

            user.FirstName = dto.FirstName;
            user.Birthday = dto.Birthday;
            user.Gender = dto.Gender;
            user.Email = dto.Email;
            user.UpdatedAt = DateTime.UtcNow;

            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            return UserMapper.ToUserDto(user);
        }
    }
}
