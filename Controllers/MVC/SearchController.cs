using Microsoft.AspNetCore.Mvc;
using PhotoForum.Models.ViewModel;
using PhotoForum.Models;
using PhotoForum.Services.Contracts;
using PhotoForum.Services;
using PhotoForum.Attributes;
using System.Web;
using System.Security.Claims;

namespace PhotoForum.Controllers.MVC
{
    //[AuthorizeRoles("user")]
    public class SearchController : Controller
    {

        private readonly IUsersService usersService;
        private readonly IPostService postsService;

        public SearchController(IUsersService usersService, IPostService postsService)
        {
            this.postsService = postsService;
            this.usersService = usersService;
        }

        [HttpGet]
        public IActionResult Index(string searchString, string userSortOrder, string postSortOrder, string option)
        {
            bool isLogged = string.IsNullOrEmpty(HttpContext.Request.Cookies["Authorization"]);

            if (!isLogged)
            {
                if (string.IsNullOrEmpty(searchString))
                {
                    return RedirectToAction("Index", "Home");
                }
                var userResults = usersService.SearchBy(searchString);
                var postResults = postsService.SearchBy(searchString);
                switch (userSortOrder)
                {
                    case "UsernameAsc":
                        userResults = userResults.OrderBy(u => u.Username).ToList();
                        break;
                    case "UsernameDesc":
                        userResults = userResults.OrderByDescending(u => u.Username).ToList();
                        break;
                }
                
                switch (postSortOrder)
                {
                    case "Newest":
                        postResults = postResults.OrderByDescending(p => p.Date).ToList(); 
                        break;
                    case "Oldest":
                        postResults = postResults.OrderBy(p => p.Date).ToList();
                        break;
                }

                var viewModel = new SearchViewModel
                {
                    Users = userResults,
                    Posts = postResults,
                    SearchString = searchString
                };

                ViewBag.SelectedOption = option;
                return View(viewModel);
            }
            return RedirectToAction("Login", "Auth");

        }
    }
}
