using EduHome.App.Context;
using EduHome.App.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.App.Controllers
{
    public class AccountController : Controller
    {
        private readonly EduHomeDbContext _context;

        public AccountController(EduHomeDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> SignUp()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(RegisterVM registerVM)
        {
            return RedirectToAction("index","home");
        }
    }
}
