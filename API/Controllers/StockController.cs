using Microsoft.AspNetCore.Mvc;
using BusinessLogic;
using BusinessLogic.DTOs.DTOsStockMovement;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly Facade _facade;

        public StockController(Facade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        public IActionResult AddStockMovement([FromBody] AddStockMovementRequest request)
        {
            try
            {
                _facade.AddStockMovement(request);
                return Ok(new { message = "Movimiento de stock agregado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<StockMovementResponse> GetStockMovementById(int id)
        {
            try
            {
                var response = _facade.GetStockMovementById(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult<List<StockMovementResponse>> GetAllStockMovements()
        {
            try
            {
                var response = _facade.GetAllStockMovements();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
