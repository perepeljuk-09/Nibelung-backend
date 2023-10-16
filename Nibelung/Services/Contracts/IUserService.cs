using Nibelung.Api.Models.Dto.User;
using Nibelung.Domain.Models;

namespace Nibelung.Api.Services.Contracts
{
    public interface IUserService
    {
        Task<UserDto> GetUserById(int userId);
        Task<UserDto> UpdateUser( int id ,UserUpdateDto dto);
    }
}
