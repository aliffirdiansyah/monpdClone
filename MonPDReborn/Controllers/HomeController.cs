using Microsoft.AspNetCore.Mvc;
using MonPDReborn.Lib.General;
using MonPDReborn.Models;
using System.Diagnostics;

namespace MonPDReborn.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null, string message)
        {
            var model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            if (statusCode.HasValue)
            {
                ViewData["StatusCode"] = statusCode.Value;
            }

            return View(model);
        }
    }
}
