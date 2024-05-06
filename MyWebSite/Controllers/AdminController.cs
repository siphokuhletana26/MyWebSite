using Microsoft.AspNetCore.Mvc;

namespace MyWebSite.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AAdminDashboard()
        {
            return View();
        }
    }
}
