using EduHome.App.Context;
using EduHome.App.Extensions;
using EduHome.Core.Entities;
using Fir.App.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public TagController(EduHomeDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Tag> Tags = await _context.Tags.Where(x => !x.IsDeleted)
                 .ToListAsync();
            return View(Tags);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tag Tag)
        {
            if (!ModelState.IsValid)
            {
                return View(Tag);
            }
            Tag.CreatedDate = DateTime.Now;
            await _context.AddAsync(Tag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Tag? Tag = await _context.Tags.Where(x => x.Id == id && !x.IsDeleted)
             .FirstOrDefaultAsync();
            if (Tag is null)
            {
                return NotFound();
            }
            return View(Tag);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id,Tag Tag)
        {
            Tag? updatedTag = await _context.Tags.Where(x => x.Id == id && !x.IsDeleted)
                  .FirstOrDefaultAsync();
            if(Tag is null)
            {
                return View(Tag);
            }
            if (!ModelState.IsValid)
            {
                return View(updatedTag);
            }
            updatedTag.Name = Tag.Name;
            updatedTag.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Remove(int id)
        {
            Tag? Tag = await _context.Tags.Where(x=>x.Id ==id && !x.IsDeleted)
                .FirstOrDefaultAsync();
            if(Tag is null)
            {
                return NotFound();
            }
            Tag.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
