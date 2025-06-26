using BusinessLogic;
using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsSupplier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SuppliersController : ControllerBase
    {
        private readonly Facade _facade;

        public SuppliersController(Facade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_SUPPLIERS))]
        public IActionResult AddSupplier([FromBody] AddSupplierRequest request)
        {
            try
            {
                var result = _facade.AddSupplier(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar agregar el proveedor." });
            }
        }

        [HttpPut]
        [Authorize(Policy = nameof(Permission.EDIT_SUPPLIERS))]
        public IActionResult UpdateSupplier([FromBody] UpdateSupplierRequest request)
        {
            try
            {
                return Ok(_facade.UpdateSupplier(request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar editar al proveedor." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_SUPPLIERS))]
        public IActionResult DeleteSupplier(int id)
        {
            try
            {
                    return Ok(_facade.DeleteSupplier(id));         
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar eliminar al proveedor." });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_SUPPLIERS))]
        public ActionResult<SupplierResponse> GetSupplierById(int id)
        {
            try
            {
                var response = _facade.GetSupplierById(id);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar obtener el proveedor." });
            }
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_SUPPLIERS))]
        public ActionResult<List<SupplierResponse>> GetAllSuppliers()
        {
            try
            {
                var response = _facade.GetAllSuppliers();
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar obtener los proveedores." });
            }
        }
        private static object FormatearError(ArgumentException ex)
        {
            var mensaje = ex.Message.Split(" (Parameter")[0];
            return new { campo = ex.ParamName, error = mensaje };
        }
    }

}
