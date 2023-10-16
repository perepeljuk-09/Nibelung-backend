using Nibelung.Domain.Enums;
using Nibelung.Domain.Models;

namespace Nibelung.Api.Models.Dto.User
{
    public class UserCreateDto
    {
        public string? FirstName { get; set; }
        public DateOnly Birthday { get; set; }
        public Gender Gender { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
