using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MonPDReborn.Models.AktivitasOP;
using System;

namespace MonPDReborn.Controllers.Aktivitas
{
    public class PendataanObjekPajakController : Controller
    {
        private readonly ILogger<PendataanObjekPajakController> _logger;

        private string URLView => $"../AktivitasOP/{nameof(PendataanObjekPajakController).Replace("Controller", "")}/";

        private string ActionName => ControllerContext.RouteData.Values["action"]?.ToString() ?? "";

        public PendataanObjekPajakController(ILogger<PendataanObjekPajakController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var model = new PendataanObjekPajakVM.Index();
                return PartialView($"{URLView}Index", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal memuat halaman Index");
                return BadRequest("Terjadi kesalahan saat memuat halaman.");
            }
        }

        public IActionResult Show(string keyword)
        {
            try
            {
                var model = new PendataanObjekPajakVM.Show(keyword);
                return PartialView($"{URLView}_Show", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal memuat data Show");
                return BadRequest("Terjadi kesalahan saat memuat data.");
            }
        }

        // ✅ Versi baru: menerima jenisPajak dan tahun
        public IActionResult Detail(string jenisPajak, int tahun)
        {
            try
            {
                var model = new PendataanObjekPajakVM.Detail(jenisPajak, tahun);
                return PartialView($"{URLView}_Detail", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal memuat data Detail");
                return BadRequest("Terjadi kesalahan saat memuat data detail.");
            }
        }
    }
}
