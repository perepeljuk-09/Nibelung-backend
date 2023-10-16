using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nibelung.Api.Models.Dto.PostLike;
using Nibelung.Api.Services.Contracts;
using System.Net;
using System.Runtime.CompilerServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Nibelung.Api.Controllers
{
    [Route("api/postlikes")]
    [ApiController]
    public class PostLikesController : ControllerBase
    {
        private readonly IPostLikeService _postLikeService;
        public PostLikesController(IPostLikeService postLikeService)
        {
            _postLikeService = postLikeService;
        }

        // POST api/postlikes
        [Authorize]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> AddLike([FromBody] PostLikeCreationDto dto)
        {
            bool result = await _postLikeService.AddLike(dto);

            return Ok(result);
        }

        // DELETE api/postlikes/5
        [Authorize]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeleteLike(int postId)
        {
            bool result = await _postLikeService.DeleteLike(postId);

            return Ok(result);
        }
    }
}
