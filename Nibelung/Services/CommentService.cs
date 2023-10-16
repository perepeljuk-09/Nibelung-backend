using Microsoft.EntityFrameworkCore;
using Nibelung.Api.Mapper;
using Nibelung.Api.Models.Dto.Comment;
using Nibelung.Api.Models.Dto.CommentAnswer;
using Nibelung.Api.Models.Dto.Pagination;
using Nibelung.Api.Services.Contracts;
using Nibelung.Domain.Models;
using Nibelung.Infrastructure.Db;
using Nibelung.Infrastructure.Models;

namespace Nibelung.Api.Services
{
    public class CommentService : ICommentService
    {
        private readonly PgContext _db;
        private readonly IUserContext _userContext;
        public CommentService(PgContext db, IUserContext userContext)
        {
            _db = db;
            _userContext = userContext;
        }
        public async Task<CommentDto> AddComment(CommentCreationDto dto)
        {
            int? userId = _userContext?.User?.UserId;

            if (userId == null)
                throw new Exception("Пользовательский айди отсутствует");

            User? user = await _db.Users.FirstOrDefaultAsync(x => x.UserId == userId);

            if (user == null)
                throw new Exception("Нет такого пользователя");

            Post? post = await _db.Posts.FirstOrDefaultAsync(x => x.PostId == dto.PostId);

            if (post == null)
                throw new Exception("Нет такого поста");

            Comment comment = CommentMapper.ToDomain(dto, userId.Value);

            await _db.Comments.AddAsync(comment);
            await _db.SaveChangesAsync();

            comment = await _db.Comments
                .Include(x => x.CommentAnswers)
                .FirstOrDefaultAsync(x => x.Id == comment.Id);

            if (comment == null)
                throw new Exception("Произошла ошибка на сервере, не удалось получить сохранённый пост");

            return CommentMapper.ToDto(comment);
        }
        public async Task<CommentDto> UpdateComment(int id, CommentUpdateDto dto)
        {
            Comment? comment = await _db.Comments
                .Include(x => x.User)
                .Include(x => x.CommentAnswers)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (comment == null)
                throw new Exception("Комментарий не найден");

            if (comment.UserId != _userContext.User?.UserId)
                throw new Exception("Нет доступа");

            comment.Content = dto.Content;
            comment.UpdatedAt = DateTime.UtcNow;

            _db.Comments.Update(comment);
            await _db.SaveChangesAsync();



            CommentDto commentDto = CommentMapper.ToDto(comment);
            return commentDto;
        }
        public async Task<bool> DeleteComment(int id)
        {
            Comment? comment = await _db.Comments.FirstOrDefaultAsync(x => x.Id == id);

            if (comment == null)
                throw new Exception("Комментарий не найден");

            if (comment.UserId != _userContext.User?.UserId)
                throw new Exception("Нет доступа");

            _db.Comments.Remove(comment);
            await _db.SaveChangesAsync();

            return true;
        }
        public async Task<CommentAnswerDto> AddCommentAnswer(CommentAnswerCreationDto dto)
        {
            Comment? comment = await _db.Comments.FirstOrDefaultAsync(x => x.Id == dto.CommentId);

            if (comment == null)
                throw new Exception("Комментарий не найден");

            if (_userContext.User == null)
                throw new Exception("Пользователь отсутствует");

            CommentAnswer answer = CommentAnswerMapper.ToDomain(dto, _userContext.User.UserId);

            await _db.CommentAnswers.AddAsync(answer);
            await _db.SaveChangesAsync();

            answer = await _db.CommentAnswers
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == answer.Id);

            CommentAnswerDto answerDto = CommentAnswerMapper.ToDto(answer!);
            return answerDto;

        }
        public async Task<CommentAnswerDto> UpdateCommentAnswer(int id, CommentAnswerUpdateDto dto)
        {
            CommentAnswer? answer = await _db.CommentAnswers.FirstOrDefaultAsync(x => x.Id == id);

            if (answer == null)
                throw new Exception("");

            if (answer.UserId != _userContext.User?.UserId)
                throw new Exception("");

            answer.Content = dto.Content;
            answer.UpdatedAt = DateTime.UtcNow;

            _db.Update(answer);
            await _db.SaveChangesAsync();

            CommentAnswerDto commentAnswerDto = CommentAnswerMapper.ToDto(answer);
            return commentAnswerDto;
        }
        public async Task<bool> DeleteCommentAnswer(int id)
        {
            CommentAnswer? answer = await _db.CommentAnswers.FirstOrDefaultAsync(x => x.Id == id);

            if (answer == null)
                throw new Exception("Ответ не найден");

            if (answer.UserId != _userContext.User?.UserId)
                throw new Exception("Нет доступа");

            _db.CommentAnswers.Remove(answer);
            await _db.SaveChangesAsync();

            return true;
        }
        public async Task<PaginationResult<CommentDto>> GetCommentsByPagination(int postId, int page, DateTime firstCommentDate)
        {
            float pageSize = 2f;

            int countComments = _db.Comments
                .Where(x => x.PostId == postId)
                .Count() - 1;

            int pagesCount = (int)Math.Ceiling(countComments / pageSize);

            PaginationResult<CommentDto> result = new PaginationResult<CommentDto>()
            {
                CurrentPage = page,
                PageSize = (int)pageSize,
                PagesCount = pagesCount,
                TotalCountItems = countComments,
                Data = new()
            };

            if (page > pagesCount) return result;

            var post = await _db.Posts
                .FirstOrDefaultAsync(x => x.PostId == postId);

            if (post == null)
                throw new Exception("Пост не найден");


            var comments = _db.Comments
                .Include(x => x.CommentAnswers)
                .Include(x => x.User)
                .OrderByDescending(x => x.AddedAt)
                .Where(x => x.PostId == postId && x.AddedAt <= firstCommentDate)
                .Skip(1)
                .Skip((page - 1) * (int)pageSize)
                .Take((int)pageSize)
                .Select(comment => new CommentDto()
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    PostId = comment.PostId,
                    UserId = comment.UserId,
                    UserFirstName = comment.User.FirstName,
                    AnswersCount = comment.CommentAnswers.Count,
                    AddedAt = comment.AddedAt,
                    UpdatedAt = comment.UpdatedAt,
                });

            result.Data = await comments.ToListAsync();

            return result;
        }
        public async Task<List<CommentAnswerDto>> GetCommentAnswersByCommentId(int commentId)
        {
            List<CommentAnswerDto> commentAnswers = await _db.CommentAnswers
                .Include(x => x.User)
                .Where(x => x.CommentId == commentId)
                .Select(x => CommentAnswerMapper.ToDto(x))
                .ToListAsync();

            return commentAnswers;
        }
    }
}
