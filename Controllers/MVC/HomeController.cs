using Microsoft.AspNetCore.Mvc;

namespace PhotoForum.Controllers.MVC
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
