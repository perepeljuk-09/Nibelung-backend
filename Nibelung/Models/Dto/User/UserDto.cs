using Nibelung.Api.Models.Dto.Post;
using Nibelung.Domain.Enums;
using Nibelung.Domain.Models;

namespace Nibelung.Api.Models.Dto.User
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public DateOnly Birthday { get; set; }
        public Gender Gender { get; set; }
        public string? Email { get; set; }
        public List<PostDto>? Posts { get; set; }
        public DateTime AddedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
