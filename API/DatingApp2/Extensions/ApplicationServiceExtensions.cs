using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using API.SignalR;
using DatingApp2.Data;
using DatingApp2.Helpers;
using DatingApp2.Interfaces;
using DatingApp2.Services;
using Microsoft.EntityFrameworkCore;

namespace DatingApp2.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<PresenceTracker>();

            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

            services.AddScoped<ITokenService, TokenService>();
            // bỏ vì dùng UnitOfWork

            //services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            services.AddScoped<IPhotoService, PhotoService>();

            services.AddScoped<LogUserActivity>();
            // bỏ vì dùng UnitOfWork
            //services.AddScoped<ILikesRepository, LikesRepository>();

            //services.AddScoped<IMessageRepository, MessageRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}
