using API.Utils;
using BusinessLogic;
using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsOrderRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderRequestController : ControllerBase
    {
        private readonly Facade _facade;
        private readonly TokenUtils _tokenUtils;

        public OrderRequestController(Facade facade, TokenUtils tokenUtils)
        {
            _facade = facade;
            _tokenUtils = tokenUtils;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_ORDER_REQUESTS))]
        public async Task<ActionResult<OrderRequestResponse>> AddOrderRequestAsync([FromBody] OrderRequestAddRequest request)
        {
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.AddOrderRequestAsync(request);
            return Created(String.Empty, result); // 201 Created con Location
        }

        [HttpPut("{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_ORDER_REQUESTS))]
        public async Task<ActionResult<OrderRequestResponse>> UpdateOrderRequestAsync(int id, [FromBody] OrderRequestUpdateRequest request)
        {
            request.Id = id;
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.UpdateOrderRequestAsync(request);
            return Ok(result); // 200 OK
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_ORDER_REQUESTS))]
        public async Task<IActionResult> DeleteOrderRequestAsync(int id)
        {
            var request = new DeleteRequest(id);
            request.setUserId(_tokenUtils.GetUserId());
            await _facade.DeleteOrderRequestAsync(request);
            return NoContent(); // 204 No Content
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_ORDER_REQUESTS))]
        public async Task<ActionResult<OrderRequestResponse>> GetOrderRequestByIdAsync(int id)
        {
            var result = await _facade.GetOrderRequestByIdAsync(id);
            return Ok(result); // 200 OK
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_ORDER_REQUESTS))]
        public async Task<ActionResult<List<OrderRequestResponse>>> GetAllOrderRequestsAsync([FromQuery] QueryOptions options)
        {
            var result = await _facade.GetAllOrderRequestsAsync(options);
            return Ok(result); // 200 OK
        }

        [HttpPut("changestatus/{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_ORDER_REQUESTS))]
        public async Task<ActionResult<OrderRequestResponse>> ChangeStatusOrderRequestAsync(int id, OrderRequestChangeStatusRequest request)
        {
            var result = await _facade.ChangeStatusOrderRequestAsync(id, request);
            return Ok(result); // 200 OK
        }
    }
}
