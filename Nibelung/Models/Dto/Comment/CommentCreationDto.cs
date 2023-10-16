using Nibelung.Domain.Models;

namespace Nibelung.Api.Models.Dto.Comment
{
    public class CommentCreationDto
    {
        public int PostId { get; set; }
        public string? Content { get; set; }
    }
}
