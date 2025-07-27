using API.Utils;
using BusinessLogic;
using BusinessLogic.Común;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsStockMovement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StockController : ControllerBase
    {
        private readonly Facade _facade;
        private readonly TokenUtils _tokenUtils;

        public StockController(Facade facade, TokenUtils tokenUtils)
        {
            _facade = facade;
            _tokenUtils = tokenUtils;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_STOCK_MOVEMENTS))]
        public async Task<ActionResult<StockMovementResponse>> AddStockMovementAsync([FromBody] AddStockMovementRequest request)
        {
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.AddStockMovementAsync(request);
            return CreatedAtAction(nameof(GetStockMovementByIdAsync), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_STOCK_MOVEMENTS))]
        public async Task<ActionResult<StockMovementResponse>> GetStockMovementByIdAsync(int id)
        {
            var result = await _facade.GetStockMovementByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_STOCK_MOVEMENTS))]
        public async Task<ActionResult<List<StockMovementResponse>>> GetAllStockMovementsAsync([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var result = await _facade.GetAllStockMovementsAsync(from, to);
            return Ok(result);
        }

        [HttpGet("shelve/{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_STOCK_MOVEMENTS))]
        public async Task<ActionResult<List<StockMovementResponse>>> GetAllStockMovementsByShelveAsync(int id, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var result = await _facade.GetAllStockMovementsByShelveAsync(id, from, to);
            return Ok(result);
        }

        [HttpGet("warehouse/{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_STOCK_MOVEMENTS))]
        public async Task<ActionResult<List<StockMovementResponse>>> GetAllStockMovementsByWarehouseAsync(int id, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var result = await _facade.GetAllStockMovementsByWarehouseAsync(id, from, to);
            return Ok(result);
        }
    }
}
