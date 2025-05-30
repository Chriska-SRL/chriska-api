using Microsoft.AspNetCore.Mvc;
using BusinessLogic;
using BusinessLogic.DTOs.DTOsDelivery;
using BusinessLogic.Dominio;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveriesController : ControllerBase
    {
        private readonly Facade _facade;

        public DeliveriesController(Facade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        public IActionResult AddDelivery([FromBody] AddDeliveryRequest request)
        {
            try
            {
                _facade.AddDelivery(request);
                return Ok(new { message = "Entrega agregada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult UpdateDelivery([FromBody] UpdateDeliveryRequest request)
        {
            try
            {
                _facade.UpdateDelivery(request);
                return Ok(new { message = "Entrega actualizada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        public IActionResult DeleteDelivery([FromBody] DeleteDeliveryRequest request)
        {
            try
            {
                _facade.DeleteDelivery(request);
                return Ok(new { message = "Entrega eliminada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("vehicles")]
        public IActionResult AddVehicle([FromBody] Vehicle vehicle)
        {
            try
            {
                _facade.AddVehicle(vehicle);
                return Ok(new { message = "Vehículo agregado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
