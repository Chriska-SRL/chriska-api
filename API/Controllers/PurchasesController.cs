using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusinessLogic;
using BusinessLogic.DTOs.DTOsPurchase;
using BusinessLogic.DTOs.DTOsPurchaseItem;
using BusinessLogic.Dominio;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PurchasesController : ControllerBase
    {
        private readonly Facade _facade;

        public PurchasesController(Facade facade)
        {
            _facade = facade;
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_PURCHASES))]
        public ActionResult<List<PurchaseResponse>> GetAllPurchases()
        {
            try
            {
                return Ok(_facade.GetAllPurchases());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch
            {
                return StatusCode(500, new { error = "Error inesperado al obtener las compras." });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_PURCHASES))]
        public ActionResult<PurchaseResponse> GetPurchaseById(int id)
        {
            try
            {
                return Ok(_facade.GetPurchaseById(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch
            {
                return StatusCode(500, new { error = $"Error inesperado al obtener la compra con id {id}." });
            }
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_PURCHASES))]
        public IActionResult AddPurchase([FromBody] AddPurchaseRequest request)
        {
            try
            {
                return Ok(_facade.AddPurchase(request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch
            {
                return StatusCode(500, new { error = "Error inesperado al agregar la compra." });
            }
        }

        [HttpPut]
        [Authorize(Policy = nameof(Permission.EDIT_PURCHASES))]
        public IActionResult UpdatePurchase([FromBody] UpdatePurchaseRequest request)
        {
            try
            {
                return Ok(_facade.UpdatePurchase(request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch
            {
                return StatusCode(500, new { error = "Error inesperado al actualizar la compra." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_PURCHASES))]
        public IActionResult DeletePurchase(int id)
        {
            try
            {
                return Ok(_facade.DeletePurchase(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { error = "Error inesperado al eliminar la compra." });
            }
        }

        // --- Purchase Items ---

        [HttpPost("items")]
        [Authorize(Policy = nameof(Permission.CREATE_PURCHASES))]
        public IActionResult AddPurchaseItem([FromBody] AddPurchaseItemRequest request)
        {
            try
            {
                return Ok(_facade.AddPurchaseItem(request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch
            {
                return StatusCode(500, new { error = "Error inesperado al agregar el ítem de compra." });
            }
        }

        [HttpPut("items")]
        [Authorize(Policy = nameof(Permission.EDIT_PURCHASES))]
        public IActionResult UpdatePurchaseItem([FromBody] UpdatePurchaseItemRequest request)
        {
            try
            {
                return Ok(_facade.UpdatePurchaseItem(request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch
            {
                return StatusCode(500, new { error = "Error inesperado al actualizar el ítem de compra." });
            }
        }

        [HttpDelete("items/{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_PURCHASES))]
        public IActionResult DeletePurchaseItem(int id)
        {
            try
            {
                return Ok(_facade.DeletePurchaseItem(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { error = "Error inesperado al eliminar el ítem de compra." });
            }
        }

        private static object FormatearError(ArgumentException ex)
        {
            var mensaje = ex.Message.Split(" (Parameter")[0];
            return new { campo = ex.ParamName, error = mensaje };
        }
    }
}
