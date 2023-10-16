

namespace Nibelung.Api.Models.Dto.Post
{
    public class NewPostDto
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
        public Nibelung.Domain.Models.Comment? FirstComment { get; set; }
        public DateTime AddedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
