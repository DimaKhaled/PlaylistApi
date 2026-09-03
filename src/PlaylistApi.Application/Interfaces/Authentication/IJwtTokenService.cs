namespace PlaylistApi.Application.Interfaces.Authentication
{
    public interface IJwtTokenService
    {
        Task<string> GenerateTokenAsync(Guid userId, string email);
    }
}
