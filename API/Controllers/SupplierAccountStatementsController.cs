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


        [HttpPost]
        [Authorize(Policy = nameof(Permission.VIEW_SUPPLIERS))]
        public async Task<ActionResult<SupplierAccountStatementResponse>> GetAccountStatementAsync([FromBody] AccountStatementRequest request)
        {
            var result = await _facade.GetSupplierAccountStatementAsync(request);
            return Ok(result);
        }
    }
}