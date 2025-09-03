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
    public class SupplierReceiptsController : ControllerBase
    {
        private readonly Facade _facade;
        private readonly TokenUtils _tokenUtils;

        public SupplierReceiptsController(Facade facade, TokenUtils tokenUtils)
        {
            _facade = facade;
            _tokenUtils = tokenUtils;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_RECEIPTS))]
        public async Task<ActionResult<SupplierReceiptResponse>> AddReceiptAsync([FromBody] SupplierReceiptAddRequest request)
        {
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.AddSupplierReceiptAsync(request);
            return Created(string.Empty, result); // 201 Created
        }

        [HttpPut("{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_RECEIPTS))]
        public async Task<ActionResult<SupplierReceiptResponse>> UpdateReceiptAsync(int id, [FromBody] ReceiptUpdateRequest request)
        {
            request.Id = id;
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.UpdateSupplierReceiptAsync(request);
            return Ok(result); // 200 OK
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_RECEIPTS))]
        public async Task<IActionResult> DeleteReceiptAsync(int id)
        {
            var request = new DeleteRequest(id);
            request.setUserId(_tokenUtils.GetUserId());
            await _facade.DeleteSupplierReceiptAsync(request);
            return NoContent(); // 204 No Content
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_RECEIPTS))]
        public async Task<ActionResult<SupplierReceiptResponse>> GetReceiptByIdAsync(int id)
        {
            var result = await _facade.GetSupplierReceiptByIdAsync(id);
            return Ok(result); // 200 OK
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_RECEIPTS))]
        public async Task<ActionResult<List<SupplierReceiptResponse>>> GetAllReceiptsAsync([FromQuery] QueryOptions options)
        {
            var result = await _facade.GetAllSupplierReceiptsAsync(options);
            return Ok(result); // 200 OK
        }
    }
}
