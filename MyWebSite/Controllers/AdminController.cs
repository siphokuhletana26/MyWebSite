using Microsoft.AspNetCore.Mvc;

namespace MyWebSite.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
<<<<<<< HEAD
        public IActionResult AdminDashboard()
=======
        public IActionResult AAdminDashboard()
>>>>>>> 3d40bff7e6b77451ae820f86dd87ff3ad1f15bcd
        {
            return View();
        }
    }
}
