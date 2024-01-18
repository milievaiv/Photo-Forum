using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoForum.Attributes;
//using PhotoForum.Attributes;
using System.Data;

namespace PhotoForum.Controllers.MVC
{
    //[Authorize(Roles = "admin")]
    [AuthorizeRoles("admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
