using EduHome.App.Context;
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
        }
    }
}
