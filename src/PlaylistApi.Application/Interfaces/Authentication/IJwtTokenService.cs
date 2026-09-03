namespace PlaylistApi.Application.Interfaces.Authentication
{
    public interface IJwtTokenService
    {
        string GenerateToken(Guid userId, string email);
    }
}
