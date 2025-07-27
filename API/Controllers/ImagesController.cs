using API.Utils;
using BusinessLogic;
using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ImagesController : ControllerBase
    {
        private readonly Facade _facade;
        private readonly TokenUtils _tokenUtils;

        public ImagesController(Facade facade, TokenUtils tokenUtils)
        {
            _facade = facade;
            _tokenUtils = tokenUtils;
        }

        [HttpPost("{entityType}/{entityId}")]
        [Authorize(Policy = "ManageImages")]
        public async Task<IActionResult> UploadImageAsync(string entityType, int entityId, IFormFile file)
        {
            var result = await _facade.UploadImageAsync(entityType, entityId, file);
            return CreatedAtAction(nameof(UploadImageAsync), new { entityType, entityId }, result);
        }

        [HttpDelete("{entityType}/{entityId}")]
        [Authorize(Policy = "ManageImages")]
        public async Task<IActionResult> DeleteImageAsync(string entityType, int entityId)
        {
            var request = new DeleteRequest(entityId);
            request.setUserId(_tokenUtils.GetUserId());

            bool deleted = await _facade.DeleteImageAsync(entityType, entityId);
            if (!deleted)
                return NotFound(new { error = "Imagen no encontrada." });

            return NoContent();
        }
    }
}
