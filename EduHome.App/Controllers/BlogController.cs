using EduHome.App.Context;
using EduHome.App.ViewModels;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Controllers
{
	public class BlogController : Controller
	{
		private readonly EduHomeDbContext _context;

		public BlogController(EduHomeDbContext context)
		{
			_context = context;
		}
		public async Task<IActionResult> Index()
		{
			IEnumerable<Blog> blogs = await _context.Blogs.Where(x => !x.IsDeleted)
				  .Include(x => x.BlogCategories)
				 .ThenInclude(x => x.Category)
					   .Include(x => x.BlogTags)
				 .ThenInclude(x => x.Tag)
				.ToListAsync();
			return View(blogs);
		}
		public async Task<IActionResult> Detail(int id)
		{
			Blog? blog = await _context.Blogs.Where(x => !x.IsDeleted)
			  .Include(x => x.BlogCategories)
				 .ThenInclude(x => x.Category)
					   .Include(x => x.BlogTags)
				 .ThenInclude(x => x.Tag)
					.FirstOrDefaultAsync();
			if (blog is null)
			{
				return NotFound();
			}
			BlogVM blogVM = new BlogVM
			{
				Blog = blog
			};

			return View(blogVM);
		}

	}
}
