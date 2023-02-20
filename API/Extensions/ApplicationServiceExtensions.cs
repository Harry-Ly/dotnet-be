using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseNpgsql(config.GetConnectionString("DefaultConnection"));
        }); // This service connects to DB using connection string supplied in the config

        services.AddCors(); // Adds CORS Policy
        services.AddScoped<ITokenService, TokenService>(); // Adds Token Service
        services.AddScoped<IUserRepository, UserRepository>(); // Adds User Repository Service
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // Single project so use this
        services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings")); // Gets config for Cloudinary

        return services;
    }
}