using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using PhotoForum.Attributes;

namespace PhotoForum.Controllers.MVC
{
    [AuthorizeRoles("user")]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
