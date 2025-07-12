using BusinessLogic;
using BusinessLogic.Dominio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly Facade _facade;

        public ImagesController(Facade facade)
        {
            _facade = facade;
        }

        [HttpPost("{entityType}/{entityId}")]
        [Authorize(Policy = "ManageImages")]
        public IActionResult UploadImage(string entityType, int entityId, IFormFile file)
        {
            try
            {
                return Ok(_facade.UploadImage(entityType, entityId, file));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error inesperado al subir la imagen." });
            }
        }

        [HttpGet("{entityType}/{entityId}")]
        [Authorize(Policy = "ManageImages")]
        public IActionResult GetImage(string entityType, int entityId)
        {
            try
            {
                var result = _facade.GetImage(entityType, entityId);
                if (result == null)
                    return NotFound(new { error = "Imagen no encontrada." });

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error inesperado al obtener la imagen." });
            }
        }

        [HttpDelete("{entityType}/{entityId}")]
        [Authorize(Policy = "ManageImages")]
        public IActionResult DeleteImage(string entityType, int entityId)
        {
            try
            {
                bool deleted = _facade.DeleteImage(entityType, entityId);
                if (!deleted)
                    return NotFound(new { error = "Imagen no encontrada." });

                return Ok(new { message = "Imagen eliminada correctamente." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error inesperado al eliminar la imagen." });
            }
        }

        private static object FormatearError(ArgumentException ex)
        {
            var mensaje = ex.Message.Split(" (Parameter")[0];
            return new { campo = ex.ParamName, error = mensaje };
        }
    }
}