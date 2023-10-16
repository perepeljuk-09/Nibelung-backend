namespace Nibelung.Api.Models.Dto.Post
{
    public class PostUpdateDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Guid? FileId { get; set; }
    }
}
