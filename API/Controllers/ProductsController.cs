using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusinessLogic;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.Dominio;

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
                var result = _facade.AddProduct(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        [Authorize(Policy = nameof(Permission.EDIT_PRODUCTS))]
        public ActionResult<ProductResponse> UpdateProduct([FromBody] UpdateProductRequest request)
        {
            try
            {
                var result = _facade.UpdateProduct(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        [Authorize(Policy = nameof(Permission.DELETE_PRODUCTS))]
        public ActionResult<ProductResponse> DeleteProduct([FromBody] DeleteProductRequest request)
        {
            try
            {
                var result = _facade.DeleteProduct(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_PRODUCTS))]
        public ActionResult<ProductResponse> GetProductById(int id)
        {
            try
            {
                var response = _facade.GetProductById(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_PRODUCTS))]
        public ActionResult<List<ProductResponse>> GetAllProducts()
        {
            try
            {
                var response = _facade.GetAllProducts();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
