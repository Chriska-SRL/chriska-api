using API.Utils;
using BusinessLogic;
using BusinessLogic.Común;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsShelve;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ShelvesController : ControllerBase
    {
        private readonly Facade _facade;
        private readonly TokenUtils _tokenUtils;

        public ShelvesController(Facade facade, TokenUtils tokenUtils)
        {
            _facade = facade;
            _tokenUtils = tokenUtils;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_WAREHOUSES))]
        public async Task<ActionResult<ShelveResponse>> AddShelveAsync([FromBody] AddShelveRequest request)
        {
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.AddShelveAsync(request);
            return Created(String.Empty, result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_WAREHOUSES))]
        public async Task<ActionResult<ShelveResponse>> UpdateShelveAsync(int id, [FromBody] UpdateShelveRequest request)
        {
            request.Id = id;
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.UpdateShelveAsync(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_WAREHOUSES))]
        public async Task<IActionResult> DeleteShelveAsync(int id)
        {
            var request = new DeleteRequest(id);
            request.setUserId(_tokenUtils.GetUserId());
            await _facade.DeleteShelveAsync(request);
            return NoContent();
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_WAREHOUSES))]
        public async Task<ActionResult<ShelveResponse>> GetShelveByIdAsync(int id)
        {
            var result = await _facade.GetShelveByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_WAREHOUSES))]
        public async Task<ActionResult<List<ShelveResponse>>> GetAllShelvesAsync([FromQuery] QueryOptions options)
        {
            var result = await _facade.GetAllShelvesAsync(options);
            return Ok(result);
        }
    }
}
