using Nibelung.Domain.Enums;

namespace Nibelung.Api.Models.Dto.User
{
    public class UserUpdateDto
    {
        public string? FirstName { get; set; }
        public DateOnly Birthday { get; set; }
        public Gender Gender { get; set; }
        public string? Email { get; set; }
    }
}
