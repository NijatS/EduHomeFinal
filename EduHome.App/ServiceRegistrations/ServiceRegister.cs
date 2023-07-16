using EduHome.App.Context;
using EduHome.App.Services.Implementations;
using EduHome.App.Services.Interfaces;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.ServiceRegistrations
{
    public static class ServiceRegister
    {
        public static void Register(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EduHomeDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("Default"));
            });
            services.AddScoped<SettingService>();
            services.AddScoped<IEmailService,EmailService>();
            services.AddIdentity<AppUser, IdentityRole>()
                    .AddDefaultTokenProviders()
                           .AddEntityFrameworkStores<EduHomeDbContext>();
        }
    }
}
