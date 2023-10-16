using Nibelung.Api.Models.Dto.Comment;
using Nibelung.Api.Models.Dto.Post;
using Nibelung.Domain.Models;

namespace Nibelung.Api.Mapper
{
    public class PostMapper
    {
        public static PostDto ToPostDto(Post post, int consumerId)
        {
            int commCount = post.Comments.Count + post.Comments.Sum(com => com.CommentAnswers?.Count).Value;

            PostDto postDto = new PostDto()
            {
                PostId = post.PostId,
                Title = post.Title,
                Description = post.Description,
                FileId = post.PostFiles?.Count > 0 ? post.PostFiles?[0].File?.Id.ToString() : null,
                CountViews = post.CountViews,
                isLiked = post.PostLikes != null ? post.PostLikes.Any(x => x.UserId == consumerId) : false,
                LikesCount = post.PostLikes != null ? post.PostLikes.Count : 0,
                CommentsCount = commCount,
                AuthorFirstName = post.User?.FirstName,
                AuthorId = post.UserId,
                Comments = post.Comments?.Select(comment => CommentMapper.ToDto(comment)).ToList(),
                AddedAt = post.AddedAt,
                UpdatedAt = post.UpdatedAt,
            };
            return postDto;
        }
        public static PostDto ToPostDto(NewPostDto newpost)
        {
            PostDto dto = new PostDto()
            {
                PostId = newpost.PostId,
                Title = newpost.Title,
                Description = newpost.Description,
                FileId = newpost.FileId,
                CountViews = newpost.CountViews,
                isLiked = newpost.isLiked,
                LikesCount = newpost.LikesCount,
                CommentsCount = newpost.CommentsCount,
                AuthorFirstName = newpost.AuthorFirstName,
                AuthorId = newpost.AuthorId,
                Comments = newpost.FirstComment != null ?
                    new List<Comment> { newpost.FirstComment }.Select(x => CommentMapper.ToDto(x)).ToList()
                    : new List<CommentDto>(),
                AddedAt = newpost.AddedAt,
                UpdatedAt = newpost.UpdatedAt,
            };
            return dto;
        }
        public static Post ToDomain(PostCreationDto dto)
        {

            if (dto.FileId != null)
            {
                Post newPost = new Post()
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    UserId = dto.UserId,
                    PostFiles = new List<PostFiles>() { new PostFiles() { FileId = dto.FileId.Value } }
                };
                return newPost;
            }
            else
            {

                Post newPost = new Post()
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    UserId = dto.UserId
                };
                return newPost;
            }
        }
    }
}
