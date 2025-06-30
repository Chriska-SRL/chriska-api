using Microsoft.AspNetCore.Mvc;
using BusinessLogic;
using BusinessLogic.DTOs.DTOsClient;
using BusinessLogic.DTOs.DTOsReceipt;
using Microsoft.AspNetCore.Authorization;
using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsProduct;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        private readonly Facade _facade;

        public ClientsController(Facade facade)
        {
            _facade = facade;
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_CLIENTS))]
        public ActionResult<List<ClientResponse>> GetAllClients()
        {
            try
            {
                return Ok(_facade.GetAllClients());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error inesperado al obtener los clientes.",ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_CLIENTS))]
        public ActionResult<ClientResponse> GetClientById(int id)
        {
            try
            {
                return Ok(_facade.GetClientById(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = $"Error inesperado al obtener el cliente con id {id}." });
            }
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_CLIENTS))]
        public IActionResult AddClient([FromBody] AddClientRequest request)
        {
            try
            {
                return Ok(_facade.AddClient(request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error inesperado al agregar el cliente." });
            }
        }

        [HttpPut]
        [Authorize(Policy = nameof(Permission.EDIT_CLIENTS))]
        public IActionResult UpdateClient([FromBody] UpdateClientRequest request)
        {
            try
            {
                return Ok(_facade.UpdateClient(request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error inesperado al actualizar el cliente." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_CLIENTS))]
        public IActionResult DeleteClient(int id)
        {
            try
            {
                return Ok(_facade.DeleteClient(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error inesperado al eliminar el cliente." });
            }
        }


        //Recibos

        [HttpPost("receipts")]
        [Authorize(Policy = nameof(Permission.CREATE_CLIENTS))]
        public IActionResult AddReceipt([FromBody] AddReceiptRequest request)
        {
            try
            {
                return Ok(_facade.AddReceipt(request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error inesperado al eliminar el recibo." });
            }
        }
        [HttpPut("receipts")]
        [Authorize(Policy = nameof(Permission.EDIT_CLIENTS))]
        public IActionResult UpdateReceipt([FromBody] UpdateReceiptRequest request)
        {
            try
            {
                return Ok(_facade.UpdateReceipt(request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error inesperado al actualizar el recibo." });
            }
        }

        [HttpDelete("receipts/{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_CLIENTS))]
        public IActionResult DeleteReceipt(int id)
        {
            try
            {
                return Ok(_facade.DeleteReceipt(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error inesperado al eliminar el recibo." });
            }
        }

        [HttpGet("receipts/{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_CLIENTS))]
        public ActionResult<ProductResponse> GetReceiptsByClientId(int id)
        {
            try
            {
                return Ok(_facade.GetReceiptById(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = $"Error inesperado al obtener el recibo con id {id}." });
            }
        }

        [HttpGet("receipts")]
        [Authorize(Policy = nameof(Permission.VIEW_CLIENTS))]
        public ActionResult<List<ReceiptResponse>> GetAllReceipts()
        {
            try
            {
                return Ok(_facade.GetAllReceipts());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error inesperado al obtener los recibos." });
            }
        }

        private static object FormatearError(ArgumentException ex)
        {
            var mensaje = ex.Message.Split(" (Parameter")[0];
            return new { campo = ex.ParamName, error = mensaje };
        }
    }
}
