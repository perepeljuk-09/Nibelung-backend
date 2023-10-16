using Microsoft.EntityFrameworkCore;
using Nibelung.Api.Mapper;
using Nibelung.Api.Models.Dto.PostLike;
using Nibelung.Api.Services.Contracts;
using Nibelung.Domain.Models;
using Nibelung.Infrastructure.Db;
using Nibelung.Infrastructure.Models;

namespace Nibelung.Api.Services
{
    public class PostLikeService : IPostLikeService
    {
        private readonly PgContext _db;
        private readonly IUserContext _userContext;
        public PostLikeService(PgContext db, IUserContext userContext)
        {
            _db = db;
            _userContext = userContext;
        }
        public async Task<bool> AddLike(PostLikeCreationDto dto)
        {

            int? userId = _userContext.User?.UserId;

            if (userId == null)
                throw new Exception("Айди пользователя отсутствует");

            User? user = await _db.Users.FirstOrDefaultAsync(x => x.UserId == userId);

            if (user == null)
                throw new Exception("Пользователь с таким айди не существует");

            Post? post = await _db.Posts.FirstOrDefaultAsync(x => x.PostId == dto.PostId);

            if(post == null)
                throw new Exception("Пост с таким айди не существует");



            PostLikes? postLike = await _db.PostLikes.FirstOrDefaultAsync(x => x.PostId == dto.PostId && x.UserId == userId);

            if (postLike != null)
                throw new Exception("Лайк уже добавлен, данным пользователем");

            PostLikes newLike = PostLikesMapper.ToDomain(dto, userId.Value);


            await _db.PostLikes.AddAsync(newLike);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteLike(int postId)
        {
            int? userId = _userContext.User?.UserId;

            if (userId == null)
                throw new Exception("Айди пользователя отсутствует");

            User? user = await _db.Users.FirstOrDefaultAsync(x => x.UserId == userId);

            if (user == null)
                throw new Exception("Пользователь с таким айди не существует");

            Post? post = await _db.Posts.FirstOrDefaultAsync(x => x.PostId == postId);

            if (post == null)
                throw new Exception("Пост с таким айди не существует");

            PostLikes? postLike = await _db.PostLikes.FirstOrDefaultAsync(x => x.PostId == postId && x.UserId == userId);

            if (postLike == null)
                throw new Exception("Невозможное действие, лайк не найден");


            _db.Remove(postLike);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
