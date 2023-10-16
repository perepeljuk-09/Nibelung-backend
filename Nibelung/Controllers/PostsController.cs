using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nibelung.Api.Models.Dto.Pagination;
using Nibelung.Api.Models.Dto.Post;
using Nibelung.Api.Services.Contracts;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Nibelung.Api.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostsController(IPostService service)
        {
            _postService = service;
        }

        // GET api/posts/5
        [Authorize]
        [ProducesResponseType(typeof(PostDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            PostDto postDto = await _postService.GetPost(id);

            return Ok(postDto);
        }

        // POST api/posts
        [Authorize]
        [ProducesResponseType(typeof(PostDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostCreationDto dto)
        {
            PostDto postDto = await _postService.CreatePost(dto);

            return Ok(postDto);
        }

        // PUT api/posts/5
        [Authorize]
        [ProducesResponseType(typeof(PostDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PostUpdateDto dto)
        {
            PostDto postDto = await _postService.UpdatePost(id, dto);

            return Ok(postDto);
        }

        // DELETE api/posts/5
        [Authorize]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await _postService.DeletePost(id);

            return Ok(result);
        }

        // GET: api/posts?page=1
        [Authorize]
        [ProducesResponseType(typeof(PaginationResult<PostDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery(Name = "page")] int page)
        {
            PaginationResult<PostDto> result = await _postService.GetPosts(page);

            return Ok(result);
        }
        // GET: api/posts/user?userId=1&page=1
        [Authorize]
        [ProducesResponseType(typeof(PaginationResult<PostDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        [HttpGet("user")]
        public async Task<IActionResult> GetUserPostsByUserId([FromQuery(Name = "userId")] int userid, [FromQuery(Name = "page")] int page)
        {
            PaginationResult<PostDto> result = await _postService.GetUserPostsByUserId(userid, page);

            return Ok(result);
        }

        // PUT api/posts/count/5
        [Authorize]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        [HttpPut("count/{id}")]
        public async Task<IActionResult> IncrementCountViews(int id)
        {
            bool result= await _postService.IncrementCountViews(id);

            return Ok(result);
        }
    }
}
