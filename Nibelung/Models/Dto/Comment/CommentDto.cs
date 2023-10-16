using Nibelung.Api.Models.Dto.CommentAnswer;
using Nibelung.Domain.Models;

namespace Nibelung.Api.Models.Dto.Comment
{
    public class CommentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? UserFirstName { get; set; }
        public int PostId { get; set; }
        public string? Content { get; set; }
        public int AnswersCount { get; set; }
        public DateTime AddedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
