using Nibelung.Api.Models.Dto.User;
using Nibelung.Domain.Models;

namespace Nibelung.Api.Mapper
{
    public class UserMapper
    {
        public static User ToDomain(UserCreateDto dto)
        {
            User user = new User();

            user.FirstName = dto.FirstName;
            user.Birthday = dto.Birthday;
            user.Gender = dto.Gender;
            user.Email = dto.Email;

            return user;
        }
        public static UserDto ToUserDto(User user)
        {
            UserDto userDto = new UserDto()
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                Birthday = user.Birthday,
                Gender = user.Gender,
                Email = user.Email,
                Posts = new (),
                AddedAt = user.AddedAt,
                UpdatedAt = user.UpdatedAt,
            };
            return userDto;
        }
    }
}
