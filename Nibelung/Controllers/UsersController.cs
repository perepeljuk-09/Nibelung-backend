using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nibelung.Api.Models.Dto.User;
using Nibelung.Api.Services.Contracts;
using Nibelung.Domain.Models;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Nibelung.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService service) 
        {
            _userService = service;
        }

        // GET api/users/5
        [Authorize]
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserDataById(int id)
        {
            UserDto user = await _userService.GetUserById(id);

            return Ok(user);
        }

        // PUT api/users/5
        [Authorize]
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserUpdateDto dto)
        {
            UserDto isUpdated = await _userService.UpdateUser(id, dto);

            return Ok(isUpdated);
        }
    }
}

// master branch