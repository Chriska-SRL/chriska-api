using API.Utils;
using BusinessLogic;
using BusinessLogic.Común;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsImage;
using BusinessLogic.DTOs.DTOsZone;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ZonesController : ControllerBase
    {
        private readonly Facade _facade;
        private readonly TokenUtils _tokenUtils;

        public ZonesController(Facade facade, TokenUtils tokenUtils)
        {
            _facade = facade;
            _tokenUtils = tokenUtils;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_ZONES))]
        public async Task<ActionResult<ZoneResponse>> AddZoneAsync([FromBody] AddZoneRequest request)
        {
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.AddZoneAsync(request);
            return Created(String.Empty, result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_ZONES))]
        public async Task<ActionResult<ZoneResponse>> UpdateZoneAsync(int id, [FromBody] UpdateZoneRequest request)
        {
            request.Id = id;
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.UpdateZoneAsync(request);
            return Ok(result); // 200 OK
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_ZONES))]
        public async Task<IActionResult> DeleteZoneAsync(int id)
        {
            var request = new DeleteRequest(id);
            request.setUserId(_tokenUtils.GetUserId());
            await _facade.DeleteZoneAsync(request);
            return NoContent(); // 204 No Content
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_ZONES))]
        public async Task<ActionResult<ZoneResponse>> GetZoneByIdAsync(int id)
        {
            var result = await _facade.GetZoneByIdAsync(id);
            return Ok(result); // 200 OK
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_ZONES))]
        public async Task<ActionResult<List<ZoneResponse>>> GetAllZonesAsync([FromQuery] QueryOptions options)
        {
            var result = await _facade.GetAllZonesAsync(options);
            return Ok(result); // 200 OK
        }

        [HttpPost("{id}/upload-image")]
        [Authorize(Policy = nameof(Permission.EDIT_ZONES))]
        public async Task<IActionResult> UploadZoneImageAsync(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("El archivo es inválido o está vacío.");
            AddImageRequest request = new AddImageRequest
            {
                EntityId = id,
                File = file,
            };
            request.setUserId(_tokenUtils.GetUserId());
            string url = await _facade.UploadZoneImageAsync(request);
            return Ok(url);
        }

        [HttpPost("{id}/delete-image")]
        [Authorize(Policy = nameof(Permission.EDIT_ZONES))]
        public async Task<IActionResult> DeleteZoneImageAsync(int id)
        {
            await _facade.DeleteZoneImageAsync(id);
            return NoContent();
        }
    }
}
