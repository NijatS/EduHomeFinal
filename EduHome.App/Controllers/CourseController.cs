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

		public async Task<IActionResult> Index(int? id = null)
		{
			if (id == null)
			{
				IEnumerable<Course> courses = await _context.Courses.Where(x => !x.IsDeleted)
						.Include(x => x.courseAssests)
						  .Include(x => x.courseCategories)
						 .ThenInclude(x => x.Category)
							   .Include(x => x.courseTags)
						 .ThenInclude(x => x.Tag)
						.ToListAsync();
                return View(courses);

            }
			else
			{
                IEnumerable<Course> courses = await _context.Courses.Where(x => !x.IsDeleted && x.courseCategories.Any(x => x.Category.Id == id))
             .Include(x => x.courseAssests)
               .Include(x => x.courseCategories)
              .ThenInclude(x => x.Category)
                    .Include(x => x.courseTags)
              .ThenInclude(x => x.Tag)
             .ToListAsync();
                return View(courses);
            }

            //x.courseCategories.Any(x => x.Category.Id == id))
        }
		public async Task<IActionResult> Detail(int id)
		{
			ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted)
				.Include(x=>x.courseCategories)
				 .ThenInclude(x=>x.Course)
				.ToListAsync();
            Course Course = await _context.Courses.Where(x => !x.IsDeleted )
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
