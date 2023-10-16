using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nibelung.Api.Models.Dto.Comment;
using Nibelung.Api.Models.Dto.CommentAnswer;
using Nibelung.Api.Models.Dto.Pagination;
using Nibelung.Api.Services.Contracts;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Nibelung.Api.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        // POST api/comments
        [Authorize]
        [ProducesResponseType(typeof(CommentDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] CommentCreationDto dto)
        {
            CommentDto commentDto = await _commentService.AddComment(dto);

            return Ok(commentDto);
        }

        // PUT api/comments/5
        [Authorize]
        [ProducesResponseType(typeof(CommentDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CommentUpdateDto dto)
        {
            CommentDto result = await _commentService.UpdateComment(id, dto);

            return Ok(result);
        }

        // DELETE api/comments/5
        [Authorize]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await _commentService.DeleteComment(id);

            return Ok(result);
        }

        // POST api/comments/answer
        [Authorize]
        [ProducesResponseType(typeof(CommentAnswerDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpPost("answer")]
        public async Task<IActionResult> AddCommentAnswer([FromBody] CommentAnswerCreationDto dto)
        {
            CommentAnswerDto commentAnswerDto = await _commentService.AddCommentAnswer(dto);

            return Ok(commentAnswerDto);
        }

        // PUT api/comments/answer/5
        [Authorize]
        [ProducesResponseType(typeof(CommentAnswerDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpPut("answer/{id}")]
        public async Task<IActionResult> UpdateCommentAnswer(int id, [FromBody] CommentAnswerUpdateDto dto)
        {
            CommentAnswerDto result = await _commentService.UpdateCommentAnswer(id, dto);

            return Ok(result);
        }

        // DELETE api/comments/answer/5
        [Authorize]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpDelete("answer/{id}")]
        public async Task<IActionResult> DeleteCommentAnswer(int id)
        {
            bool result = await _commentService.DeleteCommentAnswer(id);

            return Ok(result);
        }

        // GET api/comments/pagination?post_id=30&page=1
        [Authorize]
        [ProducesResponseType(typeof(PaginationResult<CommentDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpGet("pagination")]
        public async Task<IActionResult> GetCommentsByPagination([FromQuery(Name = "post_id")] int postId, [FromQuery(Name = "page")] int page, [FromQuery(Name = "first_comment_date")] DateTime firstCommentDate)
        {
            PaginationResult<CommentDto> result = await _commentService.GetCommentsByPagination(postId, page, firstCommentDate);

            return Ok(result);
        }

        // GET api/comments/answer/5
        [Authorize]
        [ProducesResponseType(typeof(List<CommentAnswerDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpGet("answer/{commentid}")]
        public async Task<IActionResult> GetCommentAnswersByCommentId(int commentid)
        {
            List<CommentAnswerDto> commentAnswerDtos = await _commentService.GetCommentAnswersByCommentId(commentid);

            return Ok(commentAnswerDtos);
        }
    }
}
