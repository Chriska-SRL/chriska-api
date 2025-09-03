using API.Utils;
using BusinessLogic;
using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsReceipt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientReceiptsController : ControllerBase
    {
        private readonly Facade _facade;
        private readonly TokenUtils _tokenUtils;

        public ClientReceiptsController(Facade facade, TokenUtils tokenUtils)
        {
            _facade = facade;
            _tokenUtils = tokenUtils;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_RECEIPTS))]
        public async Task<ActionResult<ClientReceiptResponse>> AddReceiptAsync([FromBody] ClientReceiptAddRequest request)
        {
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.AddReceiptAsync(request);
            return Created(string.Empty, result); // 201 Created
        }

        [HttpPut("{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_RECEIPTS))]
        public async Task<ActionResult<ClientReceiptResponse>> UpdateReceiptAsync(int id, [FromBody] ReceiptUpdateRequest request)
        {
            request.Id = id;
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.UpdateReceiptAsync(request);
            return Ok(result); // 200 OK
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_RECEIPTS))]
        public async Task<IActionResult> DeleteReceiptAsync(int id)
        {
            var request = new DeleteRequest(id);
            request.setUserId(_tokenUtils.GetUserId());
            await _facade.DeleteReceiptAsync(request);
            return NoContent(); // 204 No Content
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_RECEIPTS))]
        public async Task<ActionResult<ClientReceiptResponse>> GetReceiptByIdAsync(int id)
        {
            var result = await _facade.GetReceiptByIdAsync(id);
            return Ok(result); // 200 OK
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_RECEIPTS))]
        public async Task<ActionResult<List<ClientReceiptResponse>>> GetAllReceiptsAsync([FromQuery] QueryOptions options)
        {
            var result = await _facade.GetAllReceiptsAsync(options);
            return Ok(result); // 200 OK
        }
    }
}
