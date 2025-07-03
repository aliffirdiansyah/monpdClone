using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonPDReborn.Models.MonitoringWilayah;


namespace MonPDReborn.Controllers.MonitoringWilayah
{
    public class KontrolWilayahController : Controller
    {
        private readonly string URLView;

        private string controllerName => ControllerContext.RouteData.Values["controller"]?.ToString() ?? "";
        private string actionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        public KontrolWilayahController()
        {
            URLView = $"../MonitoringWilayah/{GetType().Name.Replace("Controller", "")}/";
        }

        public ActionResult Index()
        {
            try
            {
                ViewData["Title"] = controllerName;
                var model = new Models.MonitoringWilayah.MonitoringWilayahVM.Index();
                return View($"{URLView}{actionName}", model);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}