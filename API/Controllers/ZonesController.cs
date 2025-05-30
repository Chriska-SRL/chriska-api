using Microsoft.AspNetCore.Mvc;
using BusinessLogic;
using BusinessLogic.DTOs.DTOsZone;

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

        [HttpGet]
        public ActionResult<List<ZoneResponse>> GetAll()
        {
            try
            {
                var zones = _facade.GetAllZones();
                return Ok(zones);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ZoneResponse> GetById(int id)
        {
            try
            {
                var zone = _facade.GetZoneById(id);
                return Ok(zone);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Add([FromBody] AddZoneRequest request)
        {
            try
            {
                _facade.AddZone(request);
                return Ok(new { message = "Zona creada exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult Update([FromBody] UpdateZoneRequest request)
        {
            try
            {
                _facade.UpdateZone(request);
                return Ok(new { message = "Zona actualizada exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] DeleteZoneRequest request)
        {
            try
            {
                _facade.DeleteZone(request);
                return Ok(new { message = "Zona eliminada exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
