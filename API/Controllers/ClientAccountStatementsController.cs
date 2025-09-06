using BusinessLogic;
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


        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_CLIENTS))]
        public async Task<ActionResult<ClientAccountStatementResponse>> GetAccountStatementAsync(
        [FromRoute] int id,
        [FromQuery(Name = "from")] DateTime? from,
        [FromQuery(Name = "to")] DateTime? to)
        {
            if (from.HasValue && to.HasValue && from > to)
                return BadRequest("'from' debe ser <= 'to'.");

            var accountStatementRequest = new AccountStatementRequest
            {
                EntityId = id,
                From = from,
                To = to
            };

            var result = await _facade.GetClientAccountStatementAsync(accountStatementRequest);
            return Ok(result);
        }

    }
}