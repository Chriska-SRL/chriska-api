using API.Utils;
using BusinessLogic;
using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsDocumentClient;
using BusinessLogic.DTOs.DTOsOrder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly Facade _facade;
        private readonly TokenUtils _tokenUtils;

        public OrderController(Facade facade, TokenUtils tokenUtils)
        {
            _facade = facade;
            _tokenUtils = tokenUtils;
        }

        [HttpPut("{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_ORDERS))]
        public async Task<ActionResult<OrderResponse>> UpdateOrderAsync(int id, [FromBody] OrderUpdateRequest request)
        {
            request.Id = id;
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.UpdateOrderAsync(request);
            return Ok(result); // 200 OK
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_ORDERS))]
        public async Task<ActionResult<OrderResponse>> GetOrderByIdAsync(int id)
        {
            var result = await _facade.GetOrderByIdAsync(id);
            return Ok(result); // 200 OK
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_ORDERS))]
        public async Task<ActionResult<List<OrderResponse>>> GetAllOrdersAsync([FromQuery] QueryOptions options)
        {
            var result = await _facade.GetAllOrdersAsync(options);
            return Ok(result); // 200 OK
        }

        [HttpPut("changestatus/{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_ORDERS))]
        public async Task<ActionResult<OrderResponse>> ChangeStatusOrderAsync(int id, DocumentClientChangeStatusRequest request)
        {
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.ChangeStatusOrderAsync(id, request);
            return Ok(result); // 200 OK
        }
    }
}
