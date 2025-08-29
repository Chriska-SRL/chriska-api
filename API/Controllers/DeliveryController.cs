using API.Utils;
using BusinessLogic;
using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsDelivery;
using BusinessLogic.DTOs.DTOsDocumentClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DeliveryController : ControllerBase
    {
        private readonly Facade _facade;
        private readonly TokenUtils _tokenUtils;

        public DeliveryController(Facade facade, TokenUtils tokenUtils)
        {
            _facade = facade;
            _tokenUtils = tokenUtils;
        }

        [HttpPut("{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_DELIVERIES))]
        public async Task<ActionResult<DeliveryResponse>> UpdateDeliveryAsync(int id, [FromBody] DeliveryUpdateRequest request)
        {
            request.Id = id;
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.UpdateDeliveryAsync(request);
            return Ok(result); // 200 OK
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_DELIVERIES))]
        public async Task<ActionResult<DeliveryResponse>> GetDeliveryByIdAsync(int id)
        {
            var result = await _facade.GetDeliveryByIdAsync(id);
            return Ok(result); // 200 OK
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_DELIVERIES))]
        public async Task<ActionResult<List<DeliveryResponse>>> GetAllDeliveriesAsync([FromQuery] QueryOptions options)
        {
            var result = await _facade.GetAllDeliveriesAsync(options);
            return Ok(result); // 200 OK
        }

        [HttpPut("changestatus/{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_DELIVERIES))]
        public async Task<ActionResult<DeliveryResponse>> ChangeStatusDeliveryAsync(int id, [FromBody] DeliveryChangeStatusRequest request)
        {
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.ChangeStatusDeliveryAsync(id, request);
            return Ok(result); // 200 OK
        }

        [HttpGet("client/{clientId}/confirmed")]
        [Authorize(Policy = nameof(Permission.VIEW_DELIVERIES))]
        public async Task<ActionResult<List<DeliveryResponse>>> GetConfirmedDeliveriesByClientIdAsync(int clientId, [FromQuery] QueryOptions? options)
        {
            var result = await _facade.GetConfirmedDeliveriesByClientIdAsync(clientId, options);
            return Ok(result); // 200 OK
        }

    }
}
