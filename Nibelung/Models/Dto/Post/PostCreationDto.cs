namespace Nibelung.Api.Models.Dto.Post
{
    public class PostCreationDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int UserId { get; set; }
        public Guid? FileId { get; set; }
    }
}
