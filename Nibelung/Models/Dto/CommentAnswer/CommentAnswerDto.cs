namespace Nibelung.Api.Models.Dto.CommentAnswer
{
    public class CommentAnswerDto
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public string? UserFirstName { get; set; }
        public string? Content { get; set; }
        public DateTime AddedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
