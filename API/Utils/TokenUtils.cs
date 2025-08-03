using BusinessLogic.Domain;

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

        public List<Permission> GetPermissions()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext?.User?.Identity?.IsAuthenticated != true)
                throw new UnauthorizedAccessException("Usuario no autenticado.");

            var permissionClaims = httpContext.User.Claims
                .Where(c => c.Type == "permission")
                .Select(c => c.Value)
                .ToList();

            var permissions = new List<Permission>();

            foreach (var claimValue in permissionClaims)
            {
                if (int.TryParse(claimValue, out var intValue) &&
                    Enum.IsDefined(typeof(Permission), intValue))
                {
                    permissions.Add((Permission)intValue);
                }
            }

            return permissions;
        }

    }
}
