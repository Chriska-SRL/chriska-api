using API.Utils;
using BusinessLogic;
using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsDiscount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DiscountsController : ControllerBase
    {
        private readonly Facade _facade;
        private readonly TokenUtils _tokenUtils;

        public DiscountsController(Facade facade, TokenUtils tokenUtils)
        {
            _facade = facade;
            _tokenUtils = tokenUtils;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_DISCOUNTS))]
        public async Task<ActionResult<DiscountResponse>> AddDiscountAsync([FromBody] DiscountAddRequest request)
        {
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.AddDiscountAsync(request);
            return Created(String.Empty, result); // 201 Created con Location
        }

        [HttpPut("{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_DISCOUNTS))]
        public async Task<ActionResult<DiscountResponse>> UpdateDiscountAsync(int id, [FromBody] DiscountUpdateRequest request)
        {
            request.Id = id;
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.UpdateDiscountAsync(request);
            return Ok(result); // 200 OK
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_DISCOUNTS))]
        public async Task<IActionResult> DeleteDiscountAsync(int id)
        {
            var request = new DeleteRequest(id);
            request.setUserId(_tokenUtils.GetUserId());
            await _facade.DeleteDiscountAsync(request);
            return NoContent(); // 204 No Content
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_DISCOUNTS))]
        public async Task<ActionResult<DiscountResponse>> GetDiscountByIdAsync(int id)
        {
            var result = await _facade.GetDiscountByIdAsync(id);
            return Ok(result); // 200 OK
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_DISCOUNTS))]
        public async Task<ActionResult<List<DiscountResponse>>> GetAllDiscountsAsync([FromQuery] QueryOptions options)
        {
            var result = await _facade.GetAllDiscountsAsync(options);
            return Ok(result); // 200 OK
        }

        [HttpGet("best")]
        [Authorize(Policy = nameof(Permission.VIEW_DISCOUNTS))]
        public async Task<ActionResult<DiscountResponse?>> GetBestDiscountAsync([FromQuery] int productId, [FromQuery] int clientId)
        {
            var result = await _facade.GetBestDiscountAsync(productId, clientId);
            return Ok(result); // 200 OK
        }
    }
}
