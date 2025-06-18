using Microsoft.AspNetCore.Mvc;

namespace MonPDReborn.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
