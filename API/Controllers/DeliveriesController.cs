using API.Utils;
using BusinessLogic;
using BusinessLogic.Común;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsDelivery;
using BusinessLogic.DTOs.DTOsVehicle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DeliveriesController : ControllerBase
    {
        private readonly Facade _facade;
        private readonly TokenUtils _tokenUtils;

        public DeliveriesController(Facade facade, TokenUtils tokenUtils)
        {
            _facade = facade;
            _tokenUtils = tokenUtils;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_DELIVERIES))]
        public async Task<IActionResult> AddDeliveryAsync([FromBody] AddDeliveryRequest request)
        {
            request.AuditInfo.Created.SetAudit(_tokenUtils.GetUserId());
            await _facade.AddDeliveryAsync(request);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_DELIVERIES))]
        public async Task<IActionResult> UpdateDeliveryAsync(int id, [FromBody] UpdateDeliveryRequest request)
        {
            request.Id = id;
            request.AuditInfo.Updated.SetAudit(_tokenUtils.GetUserId());
            await _facade.UpdateDeliveryAsync(request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_DELIVERIES))]
        public async Task<IActionResult> DeleteDeliveryAsync(int id)
        {
            var request = new DeleteRequest(id);
            request.AuditInfo.Deleted.SetAudit(_tokenUtils.GetUserId());
            await _facade.DeleteDeliveryAsync(request);
            return NoContent();
        }

        [HttpPost("vehicles")]
        [Authorize(Policy = nameof(Permission.CREATE_DELIVERIES))]
        public async Task<IActionResult> AddVehicleAsync([FromBody] AddVehicleRequest request)
        {
            request.AuditInfo.Created.SetAudit(_tokenUtils.GetUserId());
            await _facade.AddVehicleAsync(request);
            return NoContent();
        }
    }
}
