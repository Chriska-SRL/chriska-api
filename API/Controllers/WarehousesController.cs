using API.Utils;
using BusinessLogic;
using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsWarehouse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WarehousesController : ControllerBase
    {
        private readonly Facade _facade;
        private readonly TokenUtils _tokenUtils;

        public WarehousesController(Facade facade, TokenUtils tokenUtils)
        {
            _facade = facade;
            _tokenUtils = tokenUtils;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_WAREHOUSES))]
        public async Task<ActionResult<WarehouseResponse>> AddWarehouseAsync([FromBody] AddWarehouseRequest request)
        {
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.AddWarehouseAsync(request);
            return Created(String.Empty, result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_WAREHOUSES))]
        public async Task<ActionResult<WarehouseResponse>> UpdateWarehouseAsync(int id, [FromBody] UpdateWarehouseRequest request)
        {
            request.Id = id;
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.UpdateWarehouseAsync(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_WAREHOUSES))]
        public async Task<IActionResult> DeleteWarehouseAsync(int id)
        {
            var request = new DeleteRequest(id);
            request.setUserId(_tokenUtils.GetUserId());
            await _facade.DeleteWarehouseAsync(request);
            return NoContent();
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_WAREHOUSES))]
        public async Task<ActionResult<WarehouseResponse>> GetWarehouseByIdAsync(int id)
        {
            var result = await _facade.GetWarehouseByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_WAREHOUSES))]
        public async Task<ActionResult<List<WarehouseResponse>>> GetAllWarehousesAsync([FromQuery] QueryOptions options)
        {
            var result = await _facade.GetAllWarehousesAsync(options);
            return Ok(result);
        }
    }
}
