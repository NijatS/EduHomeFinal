﻿using EduHome.App.Context;
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
            ViewBag.Degrees = await _context.Degrees.Where(p => !p.IsDeleted).ToListAsync();
			ViewBag.Hobbies = await _context.Hobbies.Where(p => !p.IsDeleted).ToListAsync();
			return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Teacher teacher)
        {
			ViewBag.Positions = await _context.Positions.Where(p => !p.IsDeleted).ToListAsync();
			ViewBag.Degrees = await _context.Degrees.Where(p => !p.IsDeleted).ToListAsync();
			ViewBag.Hobbies = await _context.Hobbies.Where(p => !p.IsDeleted).ToListAsync();
			if (!ModelState.IsValid)
            {
                return View(teacher);
            }
            if(teacher.DegreeId == 0 || teacher.PositionId == 0)
            {
                ModelState.AddModelError("","Every column must be fulled");
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
			foreach (var item in teacher.HobbyIds)
			{
				if (!await _context.Hobbies.AnyAsync(x => x.Id == item))
				{
					ModelState.AddModelError("", "Invalid Hobby Id");
					return View(teacher);
				}
				TeacherHobby teacherHobby = new TeacherHobby
				{
					CreatedDate = DateTime.Now,
					Teacher = teacher,
					HobbyId = item
				};
				await _context.TeacherHobbies.AddAsync(teacherHobby);
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
			ViewBag.Positions = await _context.Positions.Where(p => !p.IsDeleted).ToListAsync();
			ViewBag.Degrees = await _context.Degrees.Where(p => !p.IsDeleted).ToListAsync();
			ViewBag.Hobbies = await _context.Hobbies.Where(p => !p.IsDeleted).ToListAsync();
			Teacher? teacher = await _context.Teachers.Where(x => x.Id == id && !x.IsDeleted)
                .Include(x=>x.teacherHobbies)
                   .ThenInclude(x=>x.Hobby)
                   .Include(x=>x.Position)
                   .Include(x=>x.Degree)
             .FirstOrDefaultAsync();
            if (teacher is null)
            {
                return NotFound();
            }
            return View(teacher);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id,Teacher teacher)
        {
			ViewBag.Positions = await _context.Positions.Where(p => !p.IsDeleted).ToListAsync();
			ViewBag.Degrees = await _context.Degrees.Where(p => !p.IsDeleted).ToListAsync();
			ViewBag.Hobbies = await _context.Hobbies.Where(p => !p.IsDeleted).ToListAsync();
			Teacher? updatedTeacher = await _context.Teachers.Where(x => x.Id == id && !x.IsDeleted)
                .AsNoTracking()
			.Include(x => x.teacherHobbies)
			   .ThenInclude(x => x.Hobby)
			   .Include(x => x.Position)
			   .Include(x => x.Degree)
		 .FirstOrDefaultAsync();
			if (teacher is null)
            {
                return View(teacher);
            }
            if (!ModelState.IsValid)
            {
                return View(updatedTeacher);
            }

            if(teacher.file is not null)
            {
                if (!Helper.isImage(teacher.file))
                {
                    ModelState.AddModelError("file", "File must be image");
                    return View(teacher);
                }
                if (!Helper.isSizeOk(teacher.file, 1))
                {
                    ModelState.AddModelError("file", "Size of Image must less than 1 mb!!!");
                    return View(teacher);
                }
				Helper.RemoveImage(_environment.WebRootPath, "img/teacher/", updatedTeacher.Image);
				teacher.Image = teacher.file.CreateImage(_environment.WebRootPath, "img/teacher/");
			}
			else
			{
				teacher.Image = updatedTeacher.Image;
			}
            teacher.UpdatedDate = DateTime.Now;
            _context.Update(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Remove(int id)
        {
            Teacher? teacher = await _context.Teachers.Where(x=>x.Id ==id && !x.IsDeleted)
                .FirstOrDefaultAsync();
            if(teacher is null)
            {
                return NotFound();
            }
            teacher.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
