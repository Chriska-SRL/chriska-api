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

        [HttpPost]
        public IActionResult AddClient([FromBody] AddClientRequest request)
        {
            _facade.AddClient(request);
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateClient([FromBody] UpdateClientRequest request)
        {
            _facade.UpdateClient(request);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteClient(int id)
        {
            _facade.DeleteClient(new DeleteClientRequest { Id = id });
            return Ok();
        }

        [HttpGet]
        public IActionResult GetAllClients()
        {
            var result = _facade.GetAllClients();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetClientById(int id)
        {
            var result = _facade.GetClientById(id);
            return Ok(result);
        }

        [HttpPost("receipt")]
        public IActionResult AddReceipt([FromBody] AddReceiptRequest request)
        {
            _facade.AddReceipt(request);
            return Ok();
        }
    }
}
