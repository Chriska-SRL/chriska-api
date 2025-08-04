using API.Utils;
using BusinessLogic;
using BusinessLogic.Común;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsClient;
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
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.AddClientAsync(request);
            return Created(String.Empty, result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_CLIENTS))]
        public async Task<ActionResult<ClientResponse>> UpdateClientAsync(int id, [FromBody] UpdateClientRequest request)
        {
            request.Id = id;
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.UpdateClientAsync(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_CLIENTS))]
        public async Task<IActionResult> DeleteClientAsync(int id)
        {
            var request = new DeleteRequest(id);
            request.setUserId(_tokenUtils.GetUserId());
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

    }
}
