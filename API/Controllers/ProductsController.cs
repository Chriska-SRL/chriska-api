using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusinessLogic;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsBrand;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly Facade _facade;

        public ProductsController(Facade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_PRODUCTS))]
        public ActionResult<ProductResponse> AddProduct([FromBody] AddProductRequest request)
        {
            try
            {
                return Ok(_facade.AddProduct(request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error inesperado al agregar el producto." });
            }
        }

        [HttpPut]
        [Authorize(Policy = nameof(Permission.EDIT_PRODUCTS))]
        public ActionResult<ProductResponse> UpdateProduct([FromBody] UpdateProductRequest request)
        {
            try
            {
                return Ok(_facade.UpdateProduct(request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error inesperado al actualizar el producto." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_PRODUCTS))]
        public ActionResult<ProductResponse> DeleteProduct(int id)
        {
            try
            {
                return Ok(_facade.DeleteProduct(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error inesperado al eliminar el producto." });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_PRODUCTS))]
        public ActionResult<ProductResponse> GetProductById(int id)
        {
            try
            {
                return Ok(_facade.GetProductById(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = $"Error inesperado al obtener el producto con id {id}." });
            }
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_PRODUCTS))]
        public ActionResult<List<ProductResponse>> GetAllProducts()
        {
            try
            {
                return Ok(_facade.GetAllProducts());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error inesperado al obtener los productos." });
            }
        }

        [HttpPost("brand")]
        [Authorize(Policy = nameof(Permission.CREATE_PRODUCTS))]
        public ActionResult<BrandResponse> AddBrand([FromBody] AddBrandRequest request)
        {
            try
            {
                return Ok(_facade.AddBrand(request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error inesperado al agregar la marca." });
            }
        }

        [HttpPut("brand")]
        [Authorize(Policy = nameof(Permission.EDIT_PRODUCTS))]
        public ActionResult<BrandResponse> UpdateBrand([FromBody] UpdateBrandRequest request)
        {
            try
            {
                return Ok(_facade.UpdateBrand(request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error inesperado al actualizar la marca." });
            }
        }

        [HttpDelete("brand/{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_PRODUCTS))]
        public ActionResult<BrandResponse> DeleteBrand(int id)
        {
            try
            {
                return Ok(_facade.DeleteBrand(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error inesperado al eliminar la marca." });
            }
        }

        [HttpGet("brand/{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_PRODUCTS))]
        public ActionResult<BrandResponse> GetBrandById(int id)
        {
            try
            {
                return Ok(_facade.GetBrand(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = $"Error inesperado al obtener la marca con id {id}." });
            }
        }

        [HttpGet("brands")]
        [Authorize(Policy = nameof(Permission.VIEW_PRODUCTS))]
        public ActionResult<List<BrandResponse>> GetAllBrands()
        {
            try
            {
                return Ok(_facade.GetAllBrand());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error inesperado al obtener las marcas." });
            }
        }
        private static object FormatearError(ArgumentException ex)
        {
            var mensaje = ex.Message.Split(" (Parameter")[0];
            return new { campo = ex.ParamName, error = mensaje };
        }

    }
}
