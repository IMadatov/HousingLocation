using HousingLocation.Contexts;
using HousingLocation.Dto;
using HousingLocation.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceStatusResult;

namespace HousingLocation.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class ImageController(
        IImageService _service,
        HousingLocationContext _context
            ) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<List<PhotoDto>>> GetAllImages()
            => await FromServiceResultBaseAsync(_service.GetAllImageAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<PhotoDto>> GetByIdImage(int id)
            => await FromServiceResultBaseAsync(_service.GetByIdImageAsync(id));

        [HttpPost]
        public async Task<ActionResult<int>> PostImage()
            => await FromServiceResultBaseAsync(_service.InsertImageAsync(Request.Form.Files[0]));

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteImage(int id)
            => await FromServiceResultBaseAsync(_service.DeleteImageAsync(id));

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetImageById(int id)
        {
            var result = await _service.GetImageAsStreamAsync(id);
            
            if (result.StatusCode == 200)
            {
                var stream = result.Result;

                return File(stream, "image/jpeg");
            }

            return StatusCode(result.StatusCode, result.StatusMessage);


        }

        protected async Task<ActionResult> FromServiceResultBaseAsync<T>(Task<ServiceResultBase<T>> task)
        {
            var result = await task;
            if (result == null)
            {
                return NoContent();
            }
            var isOk = result.StatusCode < 400;
            if (isOk)
            {
                return StatusCode(result.StatusCode, result.Result);
            }
            return StatusCode(result.StatusCode, "Request failed");
        }
    }
}
