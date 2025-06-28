using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusinessLogic;
using BusinessLogic.DTOs.DTOsShelve;
using BusinessLogic.Dominio;
using BusinessLogic.Común;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ShelvesController : ControllerBase
    {
        private readonly Facade _facade;

        public ShelvesController(Facade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_WAREHOUSES))]
        public IActionResult AddShelve([FromBody] AddShelveRequest request)
        {
            try
            {
                return Ok(_facade.AddShelve(request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(Formatter.ArgumentError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar agregar la estantería." });
            }
        }

        [HttpPut]
        [Authorize(Policy = nameof(Permission.EDIT_WAREHOUSES))]
        public IActionResult UpdateShelve([FromBody] UpdateShelveRequest request)
        {
            try
            {
                return Ok(_facade.UpdateShelve(request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(Formatter.ArgumentError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar actualizar la estantería." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_WAREHOUSES))]
        public IActionResult DeleteShelve(int id)
        {
            try
            {
                return Ok(_facade.DeleteShelve(id));
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
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar eliminar la estantería." });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_WAREHOUSES))]
        public ActionResult<ShelveResponse> GetShelveById(int id)
        {
            try
            {
                return Ok(_facade.GetShelveById(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(Formatter.ArgumentError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = $"Ocurrió un error inesperado al intentar obtener la estantería con id {id}." });
            }
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_WAREHOUSES))]
        public ActionResult<List<ShelveResponse>> GetAllShelves()
        {
            try
            {
                return Ok(_facade.GetAllShelves());
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar obtener las estanterías." });
            }
        }
    }
}
