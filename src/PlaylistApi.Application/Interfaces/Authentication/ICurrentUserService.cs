namespace PlaylistApi.Application.Interfaces.Authentication
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
    }
}
