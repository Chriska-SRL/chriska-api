using Microsoft.AspNetCore.Mvc;
using BusinessLogic;
using BusinessLogic.DTOs.DTOsReturnRequest;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReturnsController : ControllerBase
    {
        private readonly Facade _facade;

        public ReturnsController(Facade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        public IActionResult AddReturnRequest([FromBody] AddReturnRequest request)
        {
            try
            {
                _facade.AddReturnRequest(request);
                return Ok(new { message = "Solicitud de devolución creada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult UpdateReturnRequest([FromBody] UpdateReturnRequest request)
        {
            try
            {
                _facade.UpdateReturnRequest(request);
                return Ok(new { message = "Solicitud de devolución actualizada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        public IActionResult DeleteReturnRequest([FromBody] DeleteReturnRequest request)
        {
            try
            {
                _facade.DeleteReturnRequest(request);
                return Ok(new { message = "Solicitud de devolución eliminada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ReturnRequestResponse> GetReturnRequestById(int id)
        {
            try
            {
                var response = _facade.GetReturnRequestById(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
