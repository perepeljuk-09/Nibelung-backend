using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Nibelung.Api.Mapper;
using Nibelung.Api.Models.Dto.Comment;
using Nibelung.Api.Models.Dto.Pagination;
using Nibelung.Api.Models.Dto.Post;
using Nibelung.Api.Services.Contracts;
using Nibelung.Domain.Models;
using Nibelung.Infrastructure.Db;
using Nibelung.Infrastructure.Models;
using System.Collections.Generic;

namespace Nibelung.Api.Services
{
    public class PostService : IPostService
    {
        private readonly PgContext _db;
        private readonly IUserContext _userContext;
        public PostService(PgContext pgContext, IUserContext userContext)
        {
            _db = pgContext;
            _userContext = userContext;
        }
        public async Task<PostDto> GetPost(int id)
        {
            int? consumerId = _userContext.User?.UserId;

            if (consumerId == null)
                throw new Exception("Нет айди пользователя");

            var post = await _db.Posts
                .Include(x => x.Comments)
                .ThenInclude(x => x.CommentAnswers)
                .Include(x => x.PostFiles)
                .ThenInclude(x => x.File)
                .Include(x => x.Comments)
                .ThenInclude(x => x.User)
                .Include(x => x.User)
                .OrderBy(x => x.UserId)
                .Select(post => new NewPostDto()
                {
                    PostId = post.PostId,
                    Title = post.Title,
                    Description = post.Description,
                    FileId = post.PostFiles.Count > 0 ? post.PostFiles[0].File.Id.ToString() : null,
                    CountViews = post.CountViews,
                    isLiked = post.PostLikes != null ? post.PostLikes.Any(x => x.UserId == consumerId) : false,
                    LikesCount = post.PostLikes != null ? post.PostLikes.Count : 0,
                    CommentsCount = post.Comments.Count + post.Comments.Sum(com => com.CommentAnswers.Count),
                    AuthorFirstName = post.User.FirstName,
                    AuthorId = post.UserId,
                    FirstComment = post.Comments.OrderByDescending(x => x.AddedAt).FirstOrDefault(),
                    AddedAt = post.AddedAt,
                    UpdatedAt = post.UpdatedAt,
                })
                .FirstOrDefaultAsync(x => x.PostId == id);


            if (post == null)
                throw new Exception("Пост не найден");

            PostDto postDto = new PostDto()
            {
                PostId = post.PostId,
                Title = post.Title,
                Description = post.Description,
                FileId = post.FileId,
                CountViews = post.CountViews,
                isLiked = post.isLiked,
                LikesCount = post.LikesCount,
                CommentsCount = post.CommentsCount,
                AuthorFirstName = post.AuthorFirstName,
                AuthorId = post.AuthorId,
                Comments = post.FirstComment != null ? new List<Comment> { post.FirstComment }.Select(x => CommentMapper.ToDto(x)).ToList() : new List<CommentDto>(),
                AddedAt = post.AddedAt,
                UpdatedAt = post.UpdatedAt,
            };


            return postDto;
        }
        public async Task<PostDto> CreatePost(PostCreationDto dto)
        {
            int? userId = _userContext.User?.UserId;

            if (userId == null) throw new Exception("Не айди пользователя");

            Post newPost = PostMapper.ToDomain(dto);

            await _db.Posts.AddAsync(newPost);
            await _db.SaveChangesAsync();

            newPost = await _db.Posts
                .Include(x => x.PostFiles)
                .ThenInclude(x => x.File)
                .Include(x => x.PostLikes)
                .Include(x => x.Comments)
                .ThenInclude(x => x.CommentAnswers)
                .ThenInclude(x => x.User)
                .Include(x => x.Comments)
                .ThenInclude(x => x.User)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.PostId == newPost.PostId);


            var a = newPost.Comments.Count + newPost.Comments.Sum(com => com.CommentAnswers?.Count ?? 0);

            PostDto postDto = PostMapper.ToPostDto(newPost, userId.Value);
            return postDto;
        }
        public async Task<PostDto> UpdatePost(int id, PostUpdateDto dto)
        {

            int? consumerId = _userContext.User?.UserId;

            if (consumerId == null)
                throw new Exception("Нет айди пользователя");

            Post? post = await _db.Posts
                .FirstOrDefaultAsync(x => x.PostId == id);

            if (post == null)
                throw new Exception("Пост не найден");

            if (post.UserId != consumerId.Value)
                throw new Exception("Нет доступа");

            post.Title = dto.Title;
            post.Description = dto.Description;

            if (dto.FileId != null)
            {
                post.PostFiles = new List<PostFiles>() { new PostFiles() { FileId = dto.FileId.Value } };
            }

            _db.Posts.Update(post);
            await _db.SaveChangesAsync();

            post = await _db.Posts
                .Include(x => x.PostLikes)
                .Include(x => x.Comments)
                .ThenInclude(x => x.CommentAnswers)
                .Include(x => x.Comments)
                .ThenInclude(x => x.User)
                .Include(x => x.PostFiles)
                .ThenInclude(x => x.File)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.PostId == id);

            if (post == null)
                throw new Exception("Пост не найден");

            PostDto postDto = PostMapper.ToPostDto(post, consumerId.Value);

            return postDto;
        }
        public async Task<bool> DeletePost(int id)
        {
            Post? post = await _db.Posts.FirstOrDefaultAsync(x => x.PostId == id);

            if (post == null)
                throw new Exception("Пост не найден");

            if (_userContext?.User?.UserId != post.UserId)
                throw new Exception("Нет доступа");

            _db.Posts.Remove(post);
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<PaginationResult<PostDto>> GetPosts(int page)
        {

            int? consumerId = _userContext.User?.UserId;

            if (consumerId == null)
                throw new Exception("consumerId without");


            float pageSize = 2f;

            int countPosts = _db.Posts.Count();

            int pagesCount = (int)Math.Ceiling(countPosts / pageSize);

            PaginationResult<PostDto> result = new PaginationResult<PostDto>()
            {
                CurrentPage = page,
                PageSize = (int)pageSize,
                PagesCount = pagesCount,
                TotalCountItems = countPosts,
                Data = new()
            };

            if (page > pagesCount) return result;

            var newpostsQuery = _db.Posts
                .Include(x => x.Comments)
                .ThenInclude(x => x.CommentAnswers)
                .Include(x => x.PostFiles)
                .ThenInclude(x => x.File)
                .Include(x => x.Comments)
                .ThenInclude(x => x.User)
                .Include(x => x.User)
                .OrderBy(x => x.AddedAt)
                .Skip((page - 1) * (int)pageSize)
                .Take((int)pageSize)
                .Select(post => new NewPostDto()
                {
                    PostId = post.PostId,
                    Title = post.Title,
                    Description = post.Description,
                    FileId = post.PostFiles.Count > 0 ? post.PostFiles[0].File.Id.ToString() : null,
                    CountViews = post.CountViews,
                    isLiked = post.PostLikes != null ? post.PostLikes.Any(x => x.UserId == consumerId) : false,
                    LikesCount = post.PostLikes != null ? post.PostLikes.Count : 0,
                    CommentsCount = post.Comments.Count + post.Comments.Sum(com => com.CommentAnswers.Count),
                    AuthorFirstName = post.User.FirstName,
                    AuthorId = post.UserId,
                    FirstComment = post.Comments.OrderByDescending(x => x.AddedAt).FirstOrDefault(),
                    AddedAt = post.AddedAt,
                    UpdatedAt = post.UpdatedAt,
                });


            List<NewPostDto> newposts = await newpostsQuery.ToListAsync();

            result.Data = newposts.Select(post => PostMapper.ToPostDto(post)).ToList();

            return result;

        }
        public async Task<PaginationResult<PostDto>> GetUserPostsByUserId(int userId, int page)
        {

            int? consumerId = _userContext.User?.UserId;

            if (consumerId == null)
                throw new Exception("consumerId without");


            float pageSize = 2f;

            int countPosts = _db.Posts
                .Where(x => x.UserId == userId)
                .Count();

            int pagesCount = (int)Math.Ceiling(countPosts / pageSize);

            PaginationResult<PostDto> result = new PaginationResult<PostDto>()
            {
                CurrentPage = page,
                PageSize = (int)pageSize,
                PagesCount = pagesCount,
                TotalCountItems = countPosts,
                Data = new()
            };

            if (page > pagesCount) return result;

            var newpostsQuery = _db.Posts
                .Include(x => x.Comments)
                .ThenInclude(x => x.CommentAnswers)
                .Include(x => x.PostFiles)
                .ThenInclude(x => x.File)
                .Include(x => x.Comments)
                .ThenInclude(x => x.User)
                .Include(x => x.User)
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.AddedAt)
                .Skip((page - 1) * (int)pageSize)
                .Take((int)pageSize)
                .Select(post => new NewPostDto()
                {
                    PostId = post.PostId,
                    Title = post.Title,
                    Description = post.Description,
                    FileId = post.PostFiles.Count > 0 ? post.PostFiles[0].File.Id.ToString() : null,
                    CountViews = post.CountViews,
                    isLiked = post.PostLikes != null ? post.PostLikes.Any(x => x.UserId == consumerId) : false,
                    LikesCount = post.PostLikes != null ? post.PostLikes.Count : 0,
                    CommentsCount = post.Comments.Count + post.Comments.Sum(com => com.CommentAnswers.Count),
                    AuthorFirstName = post.User.FirstName,
                    AuthorId = post.UserId,
                    FirstComment = post.Comments.OrderByDescending(x => x.AddedAt).FirstOrDefault(),
                    AddedAt = post.AddedAt,
                    UpdatedAt = post.UpdatedAt,
                });


            List<NewPostDto> newposts = await newpostsQuery.ToListAsync();

            result.Data = newposts.Select(post => PostMapper.ToPostDto(post)).ToList();

            return result;
        }
        public async Task<bool> IncrementCountViews(int id)
        {
            Post? post = await _db.Posts.FirstOrDefaultAsync(x => x.PostId == id);

            if (post == null)
                throw new Exception("Пост не найден");

            post.CountViews = ++post.CountViews;

            _db.Update(post);
            await _db.SaveChangesAsync();

            return true;
        }

    }
}
