using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsDocumentClient;
using BusinessLogic.DTOs.DTOsOrder;
using BusinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Utils;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly Facade _facade;
        private readonly TokenUtils _tokenUtils;

        public OrderController(Facade facade, TokenUtils tokenUtils)
        {
            _facade = facade;
            _tokenUtils = tokenUtils;
        }

        [HttpPut("changestatus/{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_ORDERS))]
        public async Task<ActionResult<OrderResponse>> ChangeStatusOrderAsync(int id, DocumentClientChangeStatusRequest request)
        {
            var result = await _facade.ChangeStatusOrderAsync(id, request);
            return Ok(result); // 200 OK
        }
    }
}
