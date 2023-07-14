using EduHome.App.Context;
using EduHome.App.Extensions;
using EduHome.Core.Entities;
using Fir.App.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CategoryController(EduHomeDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> Categories = await _context.Categories.Where(x => !x.IsDeleted)
                 .ToListAsync();
            return View(Categories);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category Category)
        {
            if (!ModelState.IsValid)
            {
                return View(Category);
            }
            Category.CreatedDate = DateTime.Now;
            await _context.AddAsync(Category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Category? Category = await _context.Categories.Where(x => x.Id == id && !x.IsDeleted)
             .FirstOrDefaultAsync();
            if (Category is null)
            {
                return NotFound();
            }
            return View(Category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id,Category Category)
        {
            Category? updatedCategory = await _context.Categories.Where(x => x.Id == id && !x.IsDeleted)
                  .FirstOrDefaultAsync();
            if(Category is null)
            {
                return View(Category);
            }
            if (!ModelState.IsValid)
            {
                return View(updatedCategory);
            }
            updatedCategory.Name = Category.Name;
            updatedCategory.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Remove(int id)
        {
            Category? Category = await _context.Categories.Where(x=>x.Id ==id && !x.IsDeleted)
                .FirstOrDefaultAsync();
            if(Category is null)
            {
                return NotFound();
            }
            Category.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
