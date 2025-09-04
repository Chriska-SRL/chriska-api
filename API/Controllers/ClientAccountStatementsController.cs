using API.Utils;
using BusinessLogic;
using BusinessLogic.DTOs;
using BusinessLogic.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.DTOs.DTOsAccountStatement;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientAccountStatementsController : ControllerBase
    {
        private readonly Facade _facade;

        public ClientAccountStatementsController(Facade facade)
        {
            _facade = facade;
        }


        [HttpPost]
        [Authorize(Policy = nameof(Permission.VIEW_CLIENTS))]
        public async Task<ActionResult<ClientAccountStatementResponse>> GetAccountStatementAsync([FromBody] AccountStatementRequest request)
        {
            var result = await _facade.GetClientAccountStatementAsync(request);
            return Ok(result);
        }
    }
}