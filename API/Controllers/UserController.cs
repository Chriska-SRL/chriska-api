using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusinessLogic;
using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsUser;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly Facade _facade;

        public UsersController(Facade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_USERS))]
        public IActionResult AddUser([FromBody] AddUserRequest request)
        {
            try
            {
                return Ok(_facade.AddUser(request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar agregar el usuario." });
            }
        }

        [HttpPut]
        [Authorize(Policy = nameof(Permission.EDIT_USERS))]
        public IActionResult UpdateUser([FromBody] UpdateUserRequest request)
        {
            try
            {
                return Ok(_facade.UpdateUser(request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar actualizar el usuario." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_USERS))]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                return Ok(_facade.DeleteUser(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar eliminar el usuario." });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_USERS))]
        public ActionResult<UserResponse> GetUserById(int id)
        {
            try
            {
                return Ok(_facade.GetUserById(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = $"Ocurrió un error inesperado al intentar obtener el usuario con id {id}." });
            }
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_USERS))]
        public ActionResult<List<UserResponse>> GetAllUsers()
        {
            try
            {
                return Ok(_facade.GetAllUsers());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar obtener los usuarios." });
            }
        }

        private static object FormatearError(ArgumentException ex)
        {
            var mensaje = ex.Message.Split(" (Parameter")[0];
            return new { campo = ex.ParamName, error = mensaje };
        }

        [HttpPost("resetpassword")]
        [Authorize(Policy = nameof(Permission.EDIT_USERS))]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                return Ok(new { message = "Contraseña restablecida correctamente:", password = _facade.ResetPassword(request.UserId, request.NewPassword)});
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("resetmypassword")]
        public IActionResult ResetMyPassword([FromBody] ResetMyPasswordRequest request)
        {
            try
            {
                UserResponse? user = _facade.Authenticate(request.Username, request.OldPassword);
                if (user == null)
                    return Unauthorized(new { error = "Credenciales inválidas" });

                return Ok(new { message = "Contraseña restablecida correctamente:", password = _facade.ResetPassword(user.Id, request.NewPassword) });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(FormatearError(ex));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
