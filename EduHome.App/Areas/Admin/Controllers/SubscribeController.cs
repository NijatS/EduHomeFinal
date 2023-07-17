using EduHome.App.Context;
using EduHome.App.Extensions;
using EduHome.Core.Entities;
using Fir.App.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubscribeController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public SubscribeController(EduHomeDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Subscribe> subscribes = await _context.Subscribes.Where(x => !x.IsDeleted)
                 .ToListAsync();
            return View(subscribes);
        }
        public async Task<IActionResult> Remove(int id)
        {
            Subscribe? subscribe = await _context.Subscribes.Where(x=>x.Id ==id && !x.IsDeleted)
                .FirstOrDefaultAsync();
            if(subscribe is null)
            {
                return NotFound();
            }
            subscribe.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
