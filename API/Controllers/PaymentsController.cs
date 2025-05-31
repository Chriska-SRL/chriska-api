using Microsoft.AspNetCore.Mvc;
using BusinessLogic;
using BusinessLogic.DTOs.DTOsPayment;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly Facade _facade;

        public PaymentsController(Facade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        public IActionResult AddPayment([FromBody] AddPaymentRequest request)
        {
            try
            {
                _facade.AddPayment(request);
                return Ok(new { message = "Pago agregado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult UpdatePayment([FromBody] UpdatePaymentRequest request)
        {
            try
            {
                _facade.UpdatePayment(request);
                return Ok(new { message = "Pago actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        public IActionResult DeletePayment([FromBody] DeletePaymentRequest request)
        {
            try
            {
                _facade.DeletePayment(request);
                return Ok(new { message = "Pago eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<PaymentResponse> GetPaymentById(int id)
        {
            try
            {
                return Ok(_facade.GetPaymentById(id));
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
