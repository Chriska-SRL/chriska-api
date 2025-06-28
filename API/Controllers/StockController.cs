using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusinessLogic;
using BusinessLogic.DTOs.DTOsStockMovement;
using BusinessLogic.Dominio;
using BusinessLogic.Común;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StockController : ControllerBase
    {
        private readonly Facade _facade;

        public StockController(Facade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_STOCK_MOVEMENTS))]
        public IActionResult AddStockMovement([FromBody] AddStockMovementRequest request)
        {
            try
            {
                return Ok(_facade.AddStockMovement(request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(Formatter.ArgumentError(ex));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar registrar el movimiento de stock." });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_STOCK_MOVEMENTS))]
        public ActionResult<StockMovementResponse> GetStockMovementById(int id)
        {
            try
            {
                return Ok(_facade.GetStockMovementById(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(Formatter.ArgumentError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = $"Ocurrió un error inesperado al intentar obtener el movimiento de stock con id {id}." });
            }
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_STOCK_MOVEMENTS))]
        public ActionResult<List<StockMovementResponse>> GetAllStockMovements(DateTime from, DateTime to)
        {
            try
            {
                return Ok(_facade.GetAllStockMovements(from, to));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(Formatter.ArgumentError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar obtener los movimientos de stock." });
            }
        }

        [HttpGet("shelve/{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_STOCK_MOVEMENTS))]
        public ActionResult<List<StockMovementResponse>> GetAllStockMovementsByShelve(int id, DateTime from, DateTime to)
        {
            try
            {
                return Ok(_facade.GetAllStockMovementsByShelve(id, from, to));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(Formatter.ArgumentError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar obtener los movimientos de stock." });
            }
        }

        [HttpGet("warehouse/{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_STOCK_MOVEMENTS))]
        public ActionResult<List<StockMovementResponse>> GetAllStockMovementsByWarehouse(int id, DateTime from, DateTime to)
        {
            try
            {
                return Ok(_facade.GetAllStockMovementsByWarehouse(id, from, to));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(Formatter.ArgumentError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar obtener los movimientos de stock." });
            }
        }
    }
}
