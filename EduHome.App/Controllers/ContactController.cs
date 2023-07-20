using EduHome.App.Context;
using EduHome.App.ViewModels;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.App.Controllers
{
    public class ContactController : Controller
    {
        private readonly EduHomeDbContext _context;

        public ContactController(EduHomeDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ContactVM contactVM = new ContactVM()
            {
                Service = _context.Services.Where(x => !x.IsDeleted)
                  .FirstOrDefault(),
            };
            return View(contactVM);
        }
        [HttpPost]
        public async Task<IActionResult> SendEmail(ContactMessage message)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Please fill all inputs qaqa";
                return RedirectToAction(nameof(Index));
            }
      
            await _context.ContactMessages.AddAsync(message);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
