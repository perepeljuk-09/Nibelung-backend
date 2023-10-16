using Nibelung.Api.Models.Dto.Comment;
using Nibelung.Domain.Models;

namespace Nibelung.Api.Models.Dto.Post
{
    public class PostDto
    {
        public int PostId { get; set; }
        public int CountViews { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? FileId { get; set; }
        public bool isLiked { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public string? AuthorFirstName { get; set; }
        public int AuthorId { get; set; }
        public List<CommentDto>? Comments { get; set; }
        public DateTime AddedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
