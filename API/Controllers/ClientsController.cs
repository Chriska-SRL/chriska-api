using API.Utils;
using BusinessLogic;
using BusinessLogic.Común;
using BusinessLogic.Dominio;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsClient;
using BusinessLogic.DTOs.DTOsReceipt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        private readonly Facade _facade;
        private readonly TokenUtils _tokenUtils;

        public ClientsController(Facade facade, TokenUtils tokenUtils)
        {
            _facade = facade;
            _tokenUtils = tokenUtils;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_CLIENTS))]
        public async Task<ActionResult<ClientResponse>> AddClientAsync([FromBody] AddClientRequest request)
        {
            request.AuditInfo.Created.SetAudit(_tokenUtils.GetUserId());
            var result = await _facade.AddClientAsync(request);
            return CreatedAtAction(nameof(GetClientByIdAsync), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_CLIENTS))]
        public async Task<ActionResult<ClientResponse>> UpdateClientAsync(int id, [FromBody] UpdateClientRequest request)
        {
            request.Id = id;
            request.AuditInfo.Updated.SetAudit(_tokenUtils.GetUserId());
            var result = await _facade.UpdateClientAsync(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_CLIENTS))]
        public async Task<IActionResult> DeleteClientAsync(int id)
        {
            var request = new DeleteRequest(id);
            request.AuditInfo.Deleted.SetAudit(_tokenUtils.GetUserId());
            await _facade.DeleteClientAsync(request);
            return NoContent();
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_CLIENTS))]
        public async Task<ActionResult<ClientResponse>> GetClientByIdAsync(int id)
        {
            var result = await _facade.GetClientByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_CLIENTS))]
        public async Task<ActionResult<List<ClientResponse>>> GetAllClientsAsync([FromQuery] QueryOptions options)
        {
            var result = await _facade.GetAllClientsAsync(options);
            return Ok(result);
        }

        // Receipts

        [HttpPost("receipts")]
        [Authorize(Policy = nameof(Permission.CREATE_CLIENTS))]
        public async Task<ActionResult<ReceiptResponse>> AddReceiptAsync([FromBody] AddReceiptRequest request)
        {
            request.AuditInfo.Created.SetAudit(_tokenUtils.GetUserId());
            var result = await _facade.AddReceiptAsync(request);
            return CreatedAtAction(nameof(GetReceiptByIdAsync), new { id = result.Id }, result);
        }

        [HttpPut("receipts/{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_CLIENTS))]
        public async Task<ActionResult<ReceiptResponse>> UpdateReceiptAsync(int id, [FromBody] UpdateReceiptRequest request)
        {
            request.Id = id;
            request.AuditInfo.Updated.SetAudit(_tokenUtils.GetUserId());
            var result = await _facade.UpdateReceiptAsync(request);
            return Ok(result);
        }

        [HttpDelete("receipts/{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_CLIENTS))]
        public async Task<IActionResult> DeleteReceiptAsync(int id)
        {
            var request = new DeleteRequest(id);
            request.AuditInfo.Deleted.SetAudit(_tokenUtils.GetUserId());
            await _facade.DeleteReceiptAsync(request);
            return NoContent();
        }

        [HttpGet("receipts/{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_CLIENTS))]
        public async Task<ActionResult<ReceiptResponse>> GetReceiptByIdAsync(int id)
        {
            var result = await _facade.GetReceiptByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("receipts")]
        [Authorize(Policy = nameof(Permission.VIEW_CLIENTS))]
        public async Task<ActionResult<List<ReceiptResponse>>> GetAllReceiptsAsync([FromQuery] QueryOptions options)
        {
            var result = await _facade.GetAllReceiptsAsync(options);
            return Ok(result);
        }
    }
}
