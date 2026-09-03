namespace PlaylistApi.Application.Interfaces.Identity
{
    public interface IIdentityService
    {
        Task<(bool Succeeded, List<string> Errors)> RegisterAsync(string email, string password);

        Task<Guid?> ValidateCredentialsAsync(string email, string password); 
    }
}
