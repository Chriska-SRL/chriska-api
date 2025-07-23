using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusinessLogic;
using BusinessLogic.DTOs.DTOsBrand;
using BusinessLogic.Común;
using BusinessLogic.Dominio;
using BusinessLogic.DTOs;
using API.Utils; // Importa la utilidad para formatear errores

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BrandsController : ControllerBase
    {
        private readonly Facade _facade;

        public BrandsController(Facade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_BRANDS))]
        public async Task<ActionResult<BrandResponse>> AddBrandAsync([FromBody] AddBrandRequest request)
        {
            try
            {
                var result = await _facade.AddBrandAsync(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ErrorUtils.FormatError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Unexpected error while adding the brand." });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_BRANDS))]
        public async Task<ActionResult<BrandResponse>> UpdateBrandAsync(int id, [FromBody] UpdateBrandRequest request)
        {
            try
            {
                request.Id = id; // Asigna el id de la ruta al DTO
                var result = await _facade.UpdateBrandAsync(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ErrorUtils.FormatError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Unexpected error while updating the brand." });
            }
        }

        [HttpDelete]
        [Authorize(Policy = nameof(Permission.DELETE_BRANDS))]
        public async Task<ActionResult<BrandResponse>> DeleteBrandAsync([FromBody] DeleteRequest request)
        {
            try
            {
                var result = await _facade.DeleteBrandAsync(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ErrorUtils.FormatError(ex));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Unexpected error while deleting the brand." });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_BRANDS))]
        public async Task<ActionResult<BrandResponse>> GetBrandByIdAsync(int id)
        {
            try
            {
                var result = await _facade.GetBrandByIdAsync(id);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ErrorUtils.FormatError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = $"Unexpected error while retrieving the brand with id {id}." });
            }
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_BRANDS))]
        public async Task<ActionResult<List<BrandResponse>>> GetAllBrandsAsync([FromQuery] QueryOptions options)
        {
            try
            {
                var result = await _facade.GetAllBrandsAsync(options);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ErrorUtils.FormatError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Unexpected error while retrieving the brands." });
            }
        }
    }
}
