using Nibelung.Api.Models.Dto.Comment;
using Nibelung.Api.Models.Dto.CommentAnswer;
using Nibelung.Api.Models.Dto.Pagination;
using Nibelung.Domain.Models;

namespace Nibelung.Api.Services.Contracts
{
    public interface ICommentService
    {
        public Task<CommentDto> AddComment(CommentCreationDto dto);
        public Task<CommentDto> UpdateComment(int id, CommentUpdateDto dto);
        public Task<bool> DeleteComment(int id);
        public Task<CommentAnswerDto> AddCommentAnswer(CommentAnswerCreationDto dto);
        public Task<CommentAnswerDto> UpdateCommentAnswer(int id, CommentAnswerUpdateDto dto);
        public Task<bool> DeleteCommentAnswer(int id);
        public Task<PaginationResult<CommentDto>> GetCommentsByPagination(int postId, int page, DateTime firstCommentDate);
        public Task<List<CommentAnswerDto>> GetCommentAnswersByCommentId(int commentId);
    }
}
