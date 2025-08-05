using API.Utils;
using BusinessLogic;
using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly Facade _facade;
        private readonly TokenUtils _tokenUtils;

        public RolesController(Facade facade, TokenUtils tokenUtils)
        {
            _facade = facade;
            _tokenUtils = tokenUtils;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_ROLES))]
        public async Task<ActionResult<RoleResponse>> AddRoleAsync([FromBody] AddRoleRequest request)
        {
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.AddRoleAsync(request);
            return Created(String.Empty,result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = nameof(Permission.EDIT_ROLES))]
        public async Task<ActionResult<RoleResponse>> UpdateRoleAsync(int id, [FromBody] UpdateRoleRequest request)
        {
            request.Id = id;
            request.setUserId(_tokenUtils.GetUserId());
            var result = await _facade.UpdateRoleAsync(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_ROLES))]
        public async Task<IActionResult> DeleteRoleAsync(int id)
        {
            DeleteRequest request = new DeleteRequest(id);
            request.setUserId(_tokenUtils.GetUserId());
            await _facade.DeleteRoleAsync(request);
            return NoContent();
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_ROLES))]
        public async Task<ActionResult<RoleResponse>> GetRoleByIdAsync(int id)
        {
            var result = await _facade.GetRoleByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_ROLES))]
        public async Task<ActionResult<List<RoleResponse>>> GetAllRolesAsync([FromQuery] QueryOptions options)
        {
            var result = await _facade.GetAllRolesAsync(options);
            return Ok(result);
        }
    }
}
