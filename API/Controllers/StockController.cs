using API.Utils;
using BusinessLogic;
using BusinessLogic.Common;
using BusinessLogic.Domain;
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
            var userId = _tokenUtils.GetUserId();
            request.setUserId(userId);

            if (request.Date == null)
            {
                request.Date = DateTime.Now;
            }
            else
            {
                var userPermissions = _tokenUtils.GetPermissions();

                if (!userPermissions.Contains(Permission.CREATE_PRODUCT_WITHDATE))
                {
                    return StatusCode(403, "No tiene permiso para establecer la fecha del movimiento de stock manualmente.");
                }
            }

            var result = await _facade.AddStockMovementAsync(request);
            return Created(String.Empty, result);
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
        public async Task<ActionResult<List<StockMovementResponse>>> GetAllStockMovementsAsync([FromQuery] QueryOptions options)
        {
            var result = await _facade.GetAllStockMovementsAsync(options);
            return Ok(result);
        }

    }
}
