using Microsoft.AspNetCore.Mvc;
using BusinessLogic;
using BusinessLogic.DTOs.DTOsDelivery;

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
            _facade.AddDelivery(request);
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateDelivery([FromBody] UpdateDeliveryRequest request)
        {
            _facade.UpdateDelivery(request);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDelivery(int id)
        {
            _facade.DeleteDelivery(new DeleteDeliveryRequest { Id = id });
            return Ok();
        }
    }
}
