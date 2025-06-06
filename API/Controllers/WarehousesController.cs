using BusinessLogic;
using BusinessLogic.DTOs.DTOsWarehouse;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarehousesController : ControllerBase
    {
        private readonly Facade _facade;

        public WarehousesController(Facade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        public IActionResult AddWarehouse([FromBody] AddWarehouseRequest request)
        {
            try
            {
                _facade.AddWarehouse(request);
                return Ok(new { message = "Almacén agregado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult UpdateWarehouse([FromBody] UpdateWarehouseRequest request)
        {
            try
            {
                _facade.UpdateWarehouse(request);
                return Ok(new { message = "Almacén actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        public IActionResult DeleteWarehouse([FromBody] DeleteWarehouseRequest request)
        {
            try
            {
                _facade.DeleteWarehouse(request);
                return Ok(new { message = "Almacén eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<WarehouseResponse> GetWarehouseById(int id)
        {
            try
            {
                var response = _facade.GetWarehouseById(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult<List<WarehouseResponse>> GetAllWarehouses()
        {
            try
            {
                var response = _facade.GetAllWarehouses();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
