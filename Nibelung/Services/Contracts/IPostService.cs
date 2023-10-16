using Nibelung.Api.Models.Dto.Pagination;
using Nibelung.Api.Models.Dto.Post;

namespace Nibelung.Api.Services.Contracts
{
    public interface IPostService
    {
        public Task<PostDto> GetPost(int id);
        public Task<PostDto> CreatePost(PostCreationDto dto);
        public Task<PostDto> UpdatePost(int id, PostUpdateDto dto);
        public Task<bool> DeletePost(int id);
        public Task<PaginationResult<PostDto>> GetPosts(int page);
        public Task<PaginationResult<PostDto>> GetUserPostsByUserId(int userId, int page);
        public Task<bool> IncrementCountViews(int id);
    }
}
