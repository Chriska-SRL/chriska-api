using Microsoft.AspNetCore.Mvc;
using BusinessLogic;
using BusinessLogic.DTOs.DTOsPurchase;
using BusinessLogic.DTOs.DTOsPurchaseItem;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchasesController : ControllerBase
    {
        private readonly Facade _facade;

        public PurchasesController(Facade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        public IActionResult AddPurchase([FromBody] AddPurchaseRequest request)
        {
            try
            {
                _facade.AddPurchase(request);
                return Ok(new { message = "Compra agregada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult UpdatePurchase([FromBody] UpdatePurchaseRequest request)
        {
            try
            {
                _facade.UpdatePurchase(request);
                return Ok(new { message = "Compra actualizada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        public IActionResult DeletePurchase([FromBody] DeletePurchaseRequest request)
        {
            try
            {
                _facade.DeletePurchase(request);
                return Ok(new { message = "Compra eliminada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<PurchaseResponse> GetPurchaseById(int id)
        {
            try
            {
                return Ok(_facade.GetPurchaseById(id));
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult<List<PurchaseResponse>> GetAllPurchases()
        {
            try
            {
                return Ok(_facade.GetAllPurchases());
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPost("item")]
        public IActionResult AddPurchaseItem([FromBody] AddPurchaseItemRequest request)
        {
            try
            {
                _facade.AddPurchaseItem(request);
                return Ok(new { message = "Ítem de compra agregado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("item")]
        public IActionResult UpdatePurchaseItem([FromBody] UpdatePurchaseItemRequest request)
        {
            try
            {
                _facade.UpdatePurchaseItem(request);
                return Ok(new { message = "Ítem de compra actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("item")]
        public IActionResult DeletePurchaseItem([FromBody] DeletePurchaseItemRequest request)
        {
            try
            {
                _facade.DeletePurchaseItem(request);
                return Ok(new { message = "Ítem de compra eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
