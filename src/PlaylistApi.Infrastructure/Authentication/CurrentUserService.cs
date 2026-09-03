using Microsoft.AspNetCore.Http;
using PlaylistApi.Application.Interfaces.Authentication;
using System.Security.Claims;

namespace PlaylistApi.Infrastructure.Authentication
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrWhiteSpace(userId))
                {
                    throw new UnauthorizedAccessException("User is not authenticated.");
                }

                if (!Guid.TryParse(userId, out var parsedUserId))
                {
                    throw new UnauthorizedAccessException("Invalid user Id");
                }

                return parsedUserId;
            }
        }
    }
}
