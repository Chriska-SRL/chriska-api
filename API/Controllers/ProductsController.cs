using Microsoft.AspNetCore.Mvc;
using BusinessLogic;
using BusinessLogic.DTOs.DTOsProduct;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly Facade _facade;

        public ProductsController(Facade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        public IActionResult AddProduct([FromBody] AddProductRequest request)
        {
            try
            {
                _facade.AddProduct(request);
                return Ok(new { message = "Producto agregado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult UpdateProduct([FromBody] UpdateProductRequest request)
        {
            try
            {
                _facade.UpdateProduct(request);
                return Ok(new { message = "Producto actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        public IActionResult DeleteProduct([FromBody] DeleteProductRequest request)
        {
            try
            {
                _facade.DeleteProduct(request);
                return Ok(new { message = "Producto eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ProductResponse> GetProductById(int id)
        {
            try
            {
                return Ok(_facade.GetProductById(id));
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult<List<ProductResponse>> GetAllProducts()
        {
            try
            {
                return Ok(_facade.GetAllProducts());
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
