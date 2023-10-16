using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Nibelung.Api.Models.Dto.Token;
using Nibelung.Api.Models.Dto.User;
using Nibelung.Api.Services.Contracts;
using System.Net;

namespace Nibelung.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {

        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _service;

        public AuthController(ILogger<AuthController> logger, IAuthService service)
        {
            _logger = logger;
            _service = service;
        }

        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost("registration")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto dto)
        {
            bool isSuccess = await _service.CreateUser(dto);
            if (!isSuccess)
                return StatusCode((int)HttpStatusCode.InternalServerError, "Произошла ошибка на сервере, попробуйте позже");

            return Ok();
        }
        [ProducesResponseType(typeof(TokensDto),(int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string),(int)HttpStatusCode.BadRequest)]
        [HttpPost("authorize")]
        public async Task<IActionResult> Authorization([FromBody] UserAuthDto dto)
        {
            TokensDto? tokensDto = await _service.Authorization(dto);

            if (tokensDto == null)
                return StatusCode((int)HttpStatusCode.BadRequest, "Неправильный логин или пароль");

            return Ok(tokensDto);
        }
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.Unauthorized)]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutTokenDto dto)
        {
            bool result = await _service.Logout(dto.RefreshToken);

            if (!result)
                return StatusCode((int)HttpStatusCode.Unauthorized, "Недопустимое действие");

            return Ok(result);
        }
        [ProducesResponseType(typeof(TokensDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpPost("tokens")]
        public async Task<IActionResult> RefreshTokens([FromBody] LogoutTokenDto dto)
        {
            TokensDto? tokensDto = await _service.RefreshTokens(dto.RefreshToken);

            if (tokensDto == null)
                return StatusCode((int)HttpStatusCode.BadRequest, "Не удалось обновить токены");

            return Ok(tokensDto);
        }
    }
}