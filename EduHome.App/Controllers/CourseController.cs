using EduHome.App.Context;
using EduHome.App.ViewModels;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Controllers
{
	public class CourseController : Controller
	{
		private readonly EduHomeDbContext _context;

		public CourseController(EduHomeDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			IEnumerable<Course> courses = await _context.Courses.Where(x => !x.IsDeleted)
				.Include(x=>x.courseAssests)
				  .Include(x=>x.courseCategories)
				 .ThenInclude(x=>x.Category)
                       .Include(x => x.courseTags)
                 .ThenInclude(x => x.Tag)
				.ToListAsync();
			return View(courses);
		}
		public async Task<IActionResult> Detail(int id)
		{
            Course Course = await _context.Courses.Where(x => !x.IsDeleted)
                    .Include(x => x.courseAssests)
                      .Include(x => x.courseCategories)
                     .ThenInclude(x => x.Category)
                           .Include(x => x.courseTags)
                     .ThenInclude(x => x.Tag)
					 .Include(x=>x.CourseLanguage)
                    .FirstOrDefaultAsync();
            if (Course is null)
			{
				return NotFound();
			}
			CourseVM courseVM = new CourseVM
			{
                Course = Course
            };

			return View(courseVM);
		}
	}
}
