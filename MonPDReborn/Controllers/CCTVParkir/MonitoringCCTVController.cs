using Microsoft.AspNetCore.Mvc;
using MonPDReborn.Lib.General;

namespace MonPDReborn.Controllers.CCTVParkir
{
    public class MonitoringCCTVController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
