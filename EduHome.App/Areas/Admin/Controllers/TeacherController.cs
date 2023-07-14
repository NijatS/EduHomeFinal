using EduHome.App.Context;
using EduHome.App.Extensions;
using EduHome.Core.Entities;
using Fir.App.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeacherController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public TeacherController(EduHomeDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Teacher> teachers = await _context.Teachers.Where(x => !x.IsDeleted)
                .Include(x=>x.Degree)
                .Include(x=>x.Position)
                 .ToListAsync();
            return View(teachers);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Positions = await _context.Positions.Where(p => !p.IsDeleted).ToListAsync();
            ViewBag.Skills = await _context.Skills.Where(p => !p.IsDeleted).ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Teacher teacher)
        {
            ViewBag.Positions = await _context.Positions.Where(p => !p.IsDeleted).ToListAsync();
            ViewBag.Skills = await _context.Skills.Where(p => !p.IsDeleted).ToListAsync();
            if (!ModelState.IsValid)
            {
                return View(teacher);
            }
            if(teacher.file is null)
            {
                ModelState.AddModelError("file", "Image must be added");
                return View(teacher);
            }
            if(!Helper.isImage(teacher.file))
            {
                ModelState.AddModelError("file", "File must be image");
                return View(teacher);
            }
            if (!Helper.isSizeOk(teacher.file,1))
            {
                ModelState.AddModelError("file", "Size of Image must less than 1 mb!!!");
                return View(teacher);
            }
            teacher.Image = teacher.file.CreateImage(_environment.WebRootPath, "img/teacher/");
            teacher.CreatedDate = DateTime.Now;
            await _context.AddAsync(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Slider? slider = await _context.Sliders.Where(x => x.Id == id && !x.IsDeleted)
             .FirstOrDefaultAsync();
            if (slider is null)
            {
                return NotFound();
            }
            return View(slider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id,Slider slider)
        {
            Slider? updatedSlider = await _context.Sliders.Where(x => x.Id == id && !x.IsDeleted)
                  .FirstOrDefaultAsync();
            if(slider is null)
            {
                return View(slider);
            }
            if (!ModelState.IsValid)
            {
                return View(updatedSlider);
            }

            if(slider.file is not null)
            {
                if (!Helper.isImage(slider.file))
                {
                    ModelState.AddModelError("file", "File must be image");
                    return View(slider);
                }
                if (!Helper.isSizeOk(slider.file, 1))
                {
                    ModelState.AddModelError("file", "Size of Image must less than 1 mb!!!");
                    return View(slider);
                }
                Helper.RemoveImage(_environment.WebRootPath, "img/slider/", updatedSlider.Image);
                updatedSlider.Image = slider.file.CreateImage(_environment.WebRootPath, "img/slider/");
            }
            updatedSlider.Title = slider.Title;
            updatedSlider.Text = slider.Text;
            updatedSlider.Link = slider.Link;
            updatedSlider.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Remove(int id)
        {
            Slider? slider = await _context.Sliders.Where(x=>x.Id ==id && !x.IsDeleted)
                .FirstOrDefaultAsync();
            if(slider is null)
            {
                return NotFound();
            }
            slider.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
