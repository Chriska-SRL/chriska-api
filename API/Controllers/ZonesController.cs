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
                _facade.AddZone(request);
                return Ok(new { message = "Zona agregada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult UpdateZone([FromBody] UpdateZoneRequest request)
        {
            try
            {
                _facade.UpdateZone(request);
                return Ok(new { message = "Zona actualizada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        public IActionResult DeleteZone([FromBody] DeleteZoneRequest request)
        {
            try
            {
                _facade.DeleteZone(request);
                return Ok(new { message = "Zona eliminada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ZoneResponse> GetZoneById(int id)
        {
            try
            {
                var response = _facade.GetZoneById(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult<List<ZoneResponse>> GetAllZones()
        {
            try
            {
                var response = _facade.GetAllZones();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
