using API.Utils;
using BusinessLogic;
using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsDocumentClient;
using BusinessLogic.DTOs.DTOsOrderRequest;
using BusinessLogic.DTOs.DTOsReturnRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReturnRequestController : ControllerBase
    {
        private readonly Facade _facade;
        private readonly TokenUtils _tokenUtils;

        public ReturnRequestController(Facade facade, TokenUtils tokenUtils)
        {
            _facade = facade;
            _tokenUtils = tokenUtils;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_RETURN_REQUESTS))]
        public async Task<ActionResult<ReturnRequestResponse>> AddReturnRequestAsync([FromBody] ReturnRequestAddRequest request)
        {
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.AddReturnRequestAsync(request);
            return Created(string.Empty, result); // 201 Created
        }

        [HttpPut("{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_RETURN_REQUESTS))]
        public async Task<ActionResult<ReturnRequestResponse>> UpdateReturnRequestAsync(int id, [FromBody] ReturnRequestUpdateRequest request)
        {
            request.Id = id;
           request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.UpdateReturnRequestAsync(request);
            return Ok(result); // 200 OK
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_RETURN_REQUESTS))]
        public async Task<IActionResult> DeleteReturnRequestAsync(int id)
        {
            var request = new DeleteRequest(id);
            request.setUserId(_tokenUtils.GetUserId());
            await _facade.DeleteReturnRequestAsync(request);
            return NoContent(); // 204 No Content
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_RETURN_REQUESTS))]
        public async Task<ActionResult<ReturnRequestResponse>> GetReturnRequestByIdAsync(int id)
        {
            var result = await _facade.GetReturnRequestByIdAsync(id);
            return Ok(result); // 200 OK
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_RETURN_REQUESTS))]
        public async Task<ActionResult<List<ReturnRequestResponse>>> GetAllReturnRequestsAsync([FromQuery] QueryOptions options)
        {
            var result = await _facade.GetAllReturnRequestsAsync(options);
            return Ok(result); // 200 OK
        }
        [HttpPut("changestatus/{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_RETURN_REQUESTS))]
        public async Task<ActionResult<ReturnRequestResponse>> ChangeStatusReturnRequestAsync(int id, DocumentClientChangeStatusRequest request)
        {
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.ChangeStatusReturnRequestAsync(id, request);
            return Ok(result); // 200 OK
        }
    }
}
