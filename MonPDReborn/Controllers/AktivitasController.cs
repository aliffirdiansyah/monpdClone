using Microsoft.AspNetCore.Mvc;

namespace MonPDReborn.Controllers
{
    public class AktivitasController : Controller
    {
        public IActionResult PemasanganAlat()
        {
            return View("PemasanganAlat/Index");
        }

        public IActionResult PemeriksaanPajak()
        {
            return View("PemeriksaanPajak/Index");
        }
        // Contoh lainnya jika ada view lain di folder Pemeriksaan/Restoran
        public IActionResult PendataanObjek()
        {
            return View("PendataanObjek/Index");
        }

        
    }
}
