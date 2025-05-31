using BusinessLogic;
using BusinessLogic.DTOs.DTOsSupplier;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly Facade _facade;

        public SuppliersController(Facade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        public IActionResult AddSupplier([FromBody] AddSupplierRequest request)
        {
            try
            {
                _facade.AddSupplier(request);
                return Ok(new { message = "Proveedor agregado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult UpdateSupplier([FromBody] UpdateSupplierRequest request)
        {
            try
            {
                _facade.UpdateSupplier(request);
                return Ok(new { message = "Proveedor actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        public IActionResult DeleteSupplier([FromBody] DeleteSupplierRequest request)
        {
            try
            {
                _facade.DeleteSupplier(request);
                return Ok(new { message = "Proveedor eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<SupplierResponse> GetSupplierById(int id)
        {
            try
            {
                var response = _facade.GetSupplierById(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult<List<SupplierResponse>> GetAllSuppliers()
        {
            try
            {
                var response = _facade.GetAllSuppliers();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
