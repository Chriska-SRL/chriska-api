using Microsoft.AspNetCore.Mvc;
using BusinessLogic;
using BusinessLogic.DTOs.DTOsClient;
using BusinessLogic.DTOs.DTOsReceipt;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly Facade _facade;

        public ClientsController(Facade facade)
        {
            _facade = facade;
        }

        [HttpGet]
        public ActionResult<List<ClientResponse>> GetAllClients()
        {
            try
            {
                return Ok(_facade.GetAllClients());
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ClientResponse> GetClientById(int id)
        {
            try
            {
                return Ok(_facade.GetClientById(id));
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult AddClient([FromBody] AddClientRequest request)
        {
            try
            {
                _facade.AddClient(request);
                return Ok(new { message = "Cliente agregado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult UpdateClient([FromBody] UpdateClientRequest request)
        {
            try
            {
                _facade.UpdateClient(request);
                return Ok(new { message = "Cliente actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        public IActionResult DeleteClient([FromBody] DeleteClientRequest request)
        {
            try
            {
                _facade.DeleteClient(request);
                return Ok(new { message = "Cliente eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("receipts")]
        public IActionResult AddReceipt([FromBody] AddReceiptRequest request)
        {
            try
            {
                _facade.AddReceipt(request);
                return Ok(new { message = "Recibo agregado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
