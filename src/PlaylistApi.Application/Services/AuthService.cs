using PlaylistApi.Application.DTOs.Auth;
using PlaylistApi.Application.Exceptions;
using PlaylistApi.Application.Interfaces.Authentication;
using PlaylistApi.Application.Interfaces.Identity;
using PlaylistApi.Application.Interfaces.Services;

namespace PlaylistApi.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IIdentityService _identityService;
        private readonly IJwtTokenService _jwtTokenService;


        public AuthService(IIdentityService identityService, IJwtTokenService jwtTokenService)
        {
            _identityService = identityService;
            _jwtTokenService = jwtTokenService;
        }


        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var result = await _identityService.RegisterAsync(request.Email, request.Password);
            if (!result.Succeeded)
            {
                throw new ValidationException(string.Join(", ", result.Errors));
            }

            var token = await _jwtTokenService.GenerateTokenAsync(result.UserId!.Value, request.Email);
            return new AuthResponse
            {
                AccessToken = token
            };
        }


        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var userId = await _identityService.ValidateCredentialsAsync(request.Email, request.Password);
            if (userId == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var token = await _jwtTokenService.GenerateTokenAsync(userId.Value, request.Email);
            return new AuthResponse
            {
                AccessToken = token
            };
        }
    }
}
