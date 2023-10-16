using Nibelung.Api.Models.Dto.CommentAnswer;
using Nibelung.Domain.Models;

namespace Nibelung.Api.Mapper
{
    public class CommentAnswerMapper
    {
        public static CommentAnswer ToDomain (CommentAnswerCreationDto dto, int userId)
        {
            CommentAnswer answer = new CommentAnswer()
            {
                CommentId = dto.CommentId,
                UserId = userId,
                Content = dto.Content,
            };
            return answer;
        }
        public static CommentAnswerDto ToDto(CommentAnswer answer)
        {
            CommentAnswerDto dto = new CommentAnswerDto()
            {
                CommentId = answer.CommentId,
                UserId = answer.UserId,
                UserFirstName = answer.User?.FirstName,
                Content = answer.Content,
                AddedAt = answer.AddedAt,
                UpdatedAt = answer.UpdatedAt,
                Id = answer.Id
            };
            return dto;
        }
    }
}
