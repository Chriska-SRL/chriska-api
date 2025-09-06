using API.Utils;
using BusinessLogic;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsAccountStatement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SupplierAccountStatementsController : ControllerBase
    {
        private readonly Facade _facade;

        public SupplierAccountStatementsController(Facade facade)
        {
            _facade = facade;
        }


        [HttpGet("{id:int}")]
        [Authorize(Policy = nameof(Permission.VIEW_SUPPLIERS))]
        public async Task<ActionResult<SupplierAccountStatementResponse>> GetAccountStatementAsync(
        [FromRoute] int id,
        [FromQuery(Name = "from")] DateTime? from,
        [FromQuery(Name = "to")] DateTime? to)
        {
            if (from.HasValue && to.HasValue && from > to)
                return BadRequest("'from' debe ser <= 'to'.");

            var request = new AccountStatementRequest
            {
                EntityId = id,
                From = from,
                To = to
            };

            var result = await _facade.GetSupplierAccountStatementAsync(request);
            return Ok(result);
        }

    }
}