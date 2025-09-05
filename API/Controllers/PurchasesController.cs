using API.Utils;
using BusinessLogic;
using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsDocumentClient;
using BusinessLogic.DTOs.DTOsOrderRequest;
using BusinessLogic.DTOs.DTOsPurchase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PurchasesController : ControllerBase
    {
        private readonly Facade _facade;
        private readonly TokenUtils _tokenUtils;

        public PurchasesController(Facade facade, TokenUtils tokenUtils)
        {
            _facade = facade;
            _tokenUtils = tokenUtils;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_PURCHASES))]
        public async Task<ActionResult<PurchaseResponse>> AddPurchaseAsync([FromBody] PurchaseAddRequest request)
        {
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.AddPurchaseAsync(request);
            return Created(string.Empty, result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_PURCHASES))]
        public async Task<ActionResult<PurchaseResponse>> UpdatePurchaseAsync(int id, [FromBody] PurchaseUpdateRequest request)
        {
            request.Id = id;
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.UpdatePurchaseAsync(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_PURCHASES))]
        public async Task<ActionResult<PurchaseResponse>> GetPurchaseByIdAsync(int id)
        {
            var result = await _facade.GetPurchaseByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_PURCHASES))]
        public async Task<ActionResult<List<PurchaseResponse>>> GetAllPurchasesAsync([FromQuery] QueryOptions options)
        {
            var result = await _facade.GetAllPurchasesAsync(options);
            return Ok(result);
        }


        [HttpPut("changestatus/{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_PURCHASES))]
        public async Task<ActionResult<PurchaseResponse>> ChangeStatusPurchaseAsync(int id, DocumentClientChangeStatusRequest request)
        {
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.ChangeStatusPurchaseAsync(id, request);
            return Ok(result); // 200 OK
        }
    }
}