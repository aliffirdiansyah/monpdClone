using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardWeb;
using DashBoardDevExp.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DashBoardDevExp.Controllers
{
    public class HomeController : DashboardController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DashboardConfigurator _configurator;
        private readonly IWebHostEnvironment _hostEnvironment;

        public HomeController(
                    ILogger<HomeController> logger,
        IWebHostEnvironment hostEnvironment,
            DashboardConfigurator configurator,
            IDataProtectionProvider dataProtectionProvider = null
            ) : base(configurator, dataProtectionProvider)
        {
            _logger = logger;
            _configurator = configurator;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            //var fileProvider = _hostEnvironment.ContentRootFileProvider;
            //DashboardFileStorage dashboardFileStorage = new DashboardFileStorage(fileProvider.GetFileInfo("Data/Dashboard").PhysicalPath);
            //_configurator.SetDashboardStorage(dashboardFileStorage);


            return View();
        }

        public IActionResult Edit()
        {
            //var fileProvider = _hostEnvironment.ContentRootFileProvider;
            //DashboardFileStorage dashboardFileStorage = new DashboardFileStorage(fileProvider.GetFileInfo("Data/Dashboard").PhysicalPath);
            //_configurator.SetDashboardStorage(dashboardFileStorage);


            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
