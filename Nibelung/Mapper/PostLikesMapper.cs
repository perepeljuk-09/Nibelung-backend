using Nibelung.Api.Models.Dto.PostLike;
using Nibelung.Domain.Models;

namespace Nibelung.Api.Mapper
{
    public class PostLikesMapper
    {
        public static PostLikes ToDomain(PostLikeCreationDto dto, int userId)
        {
            PostLikes postLike = new PostLikes()
            {
                PostId = dto.PostId,
                UserId = userId,
            };
            return postLike;
        }
    }
}
