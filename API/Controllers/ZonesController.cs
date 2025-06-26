using BusinessLogic;
using BusinessLogic.DTOs.DTOsZone;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZonesController : ControllerBase
    {
        private readonly Facade _facade;

        public ZonesController(Facade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        public IActionResult AddZone([FromBody] AddZoneRequest request)
        {
            try
            {
                return Ok(_facade.AddZone(request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error inesperado al agregar la zona." });
            }
        }

        [HttpPut]
        public IActionResult UpdateZone([FromBody] UpdateZoneRequest request)
        {
            try
            {
                return Ok(_facade.UpdateZone(request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error inesperado al actualizar la zona." });
            }
        }

        [HttpDelete]
        public IActionResult DeleteZone(int id)
        {
            try
            {
                return Ok(_facade.DeleteZone(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error inesperado al eliminar la zona." });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ZoneResponse> GetZoneById(int id)
        {
            try
            {
                return Ok(_facade.GetZoneById(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error inesperado al obtener la zona." });
            }
        }

        [HttpGet]
        public ActionResult<List<ZoneResponse>> GetAllZones()
        {
            try
            {
                return Ok(_facade.GetAllZones());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error inesperado al obtener las zonas." });
            }
        }
        private static object FormatearError(ArgumentException ex)
        {
            var mensaje = ex.Message.Split(" (Parameter")[0];
            return new { campo = ex.ParamName, error = mensaje };
        }
    }
}
