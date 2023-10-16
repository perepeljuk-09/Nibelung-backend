using Nibelung.Api.Models.Dto.PostLike;

namespace Nibelung.Api.Services.Contracts
{
    public interface IPostLikeService
    {
        public Task<bool> AddLike(PostLikeCreationDto dto);
        public Task<bool> DeleteLike(int postId);
    }
}
