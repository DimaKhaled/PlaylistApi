using Microsoft.AspNetCore.Identity;
using PlaylistApi.Application.Interfaces.Identity;

namespace PlaylistApi.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        public async Task<(bool Succeeded, Guid? UserId, List<string> Errors)> RegisterAsync(string email, string password)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                return (true, user.Id, []);
            }

            var errors = result.Errors.Select(e => e.Description);
            return (false, null, errors.ToList());
        }



        public async Task<Guid?> ValidateCredentialsAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswordValid)
            {
                return null;
            }
            return user.Id;
        }
    }
}
