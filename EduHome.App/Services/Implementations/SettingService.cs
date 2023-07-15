using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;

namespace EduHome.App.Services.Implementations
{
    public class SettingService
    {
        public readonly EduHomeDbContext _context;

        public SettingService(EduHomeDbContext context)
        {
            _context = context;
        }

        public async Task<Service> GetSetting()
        {
            Service? service = await _context.Services.FirstOrDefaultAsync();
            return service;
        }
    }
}
