using Microsoft.Extensions.DependencyInjection;
using PlaylistApi.Application.Interfaces.Services;
using PlaylistApi.Application.Services;
namespace PlaylistApi.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IPlaylistService, PlaylistService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ISongService, SongService>();
            return services;
        }
    }
}
