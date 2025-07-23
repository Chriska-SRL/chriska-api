using System.Security.Claims;

namespace API.Utils
{
    public class TokenUtils
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenUtils(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetUserId()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext?.User?.Identity?.IsAuthenticated != true)
                throw new UnauthorizedAccessException("Usuario no autenticado.");

            var userIdClaim = httpContext.User.FindFirst("userId");

            if (userIdClaim == null)
                throw new InvalidOperationException("No se encontró el claim 'userId' en el token.");

            if (!int.TryParse(userIdClaim.Value, out int userId))
                throw new InvalidOperationException("El claim 'userId' no es un número válido.");

            return userId;
        }
    }
}
