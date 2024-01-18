using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using PhotoForum.Attributes;
using Microsoft.EntityFrameworkCore;
using PhotoForum.Services.Contracts;
using PhotoForum.Services;
using PhotoForum.Models;
using PhotoForum.Models.ViewModel;

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
