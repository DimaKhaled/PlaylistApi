using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PlaylistApi.Application.Interfaces.Authentication;
using PlaylistApi.Application.Interfaces.ExternalServices;
using PlaylistApi.Application.Interfaces.Identity;
using PlaylistApi.Application.Interfaces.Repositories;
using PlaylistApi.Infrastructure.Authentication;
using PlaylistApi.Infrastructure.ExternalServices.iTunes;
using PlaylistApi.Infrastructure.Identity;
using PlaylistApi.Infrastructure.Persistence;
using PlaylistApi.Infrastructure.Persistence.Repositories;
using System.Text;

namespace PlaylistApi.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;

            }).AddEntityFrameworkStores<AppDbContext>();

            services.AddScoped<IIdentityService, IdentityService>();

            var jwtKey = configuration["Jwt:Key"]!;
            var jwtIssuer = configuration["Jwt:Issuer"]!;
            var jwtAudience = configuration["Jwt:Audience"]!;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });

            services.AddScoped<IJwtTokenService, JwtTokenService>();

            services.AddHttpContextAccessor();

            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddScoped<IPlaylistRepository, PlaylistRepository>();

            services.AddScoped<ISongRepository, SongRepository>();

            services.AddHttpClient<IMusicService, ITunesMusicService>(client =>
            {
                client.BaseAddress = new Uri("https://itunes.apple.com/");
            });

            return services;
        }
    }
}
