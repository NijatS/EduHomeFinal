using Microsoft.AspNetCore.Mvc;

namespace EduHome.App.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
