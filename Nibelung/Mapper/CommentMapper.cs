using Nibelung.Api.Models.Dto.Comment;
using Nibelung.Api.Models.Dto.CommentAnswer;
using Nibelung.Domain.Models;

namespace Nibelung.Api.Mapper
{
    public class CommentMapper
    {
        public static Comment ToDomain(CommentCreationDto dto, int userId)
        {
            Comment comment = new Comment()
            {
                PostId = dto.PostId,
                UserId = userId,
                Content = dto.Content
            };
            return comment;
        }
        public static CommentDto ToDto(Comment comment)
        {
            CommentDto dto = new CommentDto()
            {
                Id = comment.Id,
                Content = comment.Content,
                PostId = comment.PostId,
                UserId = comment.UserId,
                UserFirstName = comment.User?.FirstName,
                AnswersCount = comment.CommentAnswers?.Count ?? 0,
                AddedAt = comment.AddedAt,
                UpdatedAt = comment.UpdatedAt,
            };

            return dto;
        }
    }
}
