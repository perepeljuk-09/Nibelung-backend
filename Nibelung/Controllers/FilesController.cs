using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nibelung.Api.Services;
using Nibelung.Api.Services.Contracts;
using Nibelung.Infrastructure.Configs;
using System.Net;
using System.Net.Mime;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Nibelung.Api.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;
        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }
        // GET api/files/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFile(Guid id)
        {
            byte[] file = await _fileService.GetFileDataAsync(id);
            return File(file, "image/png");
        }

        // POST api/files
        [Authorize]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(bool),(int)HttpStatusCode.InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile formFile)
        {
            if (formFile.Length > FilesConfig.MaxLenght)
                return BadRequest("Превышено максимальное допустимое значениe размера файла");

            string result = await _fileService.Upload(formFile);

            if (result == "Не удалось выполнить запись файла")
                return StatusCode((int)HttpStatusCode.InternalServerError,"Не удалось выполнить запись файла");

            return Ok(result);
        }

        // DELETE api/files/5
        [Authorize]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        [HttpDelete("{fileid}")]
        public async Task<IActionResult> Delete(Guid fileid)
        {
            bool result = await _fileService.DeleteFile(fileid);

            return Ok(result);
        }
    }
}
