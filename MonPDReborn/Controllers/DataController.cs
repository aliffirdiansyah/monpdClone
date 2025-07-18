using Microsoft.AspNetCore.Mvc;
using MonPDReborn.Lib.General;

namespace MonPDReborn.Controllers
{
    public class DataController : BaseController
    {
        public IActionResult ProfilOp()
        {
            return View("ProfilOp/Index");
        }

        public IActionResult ProfilPembayaran()
        {
            return View("ProfilPembayaran/Index");
        }

        public IActionResult ProfilPotensi()
        {
            return View("ProfilPotensi/Index");
        }

        public IActionResult ProfilSpasial()
        {
            return View("ProfilSpasial/Index");
        }

        public IActionResult ProfilTarget()
        {
            return View("ProfilTarget/Index");
        }
        public IActionResult Pelaporan()
        {
            return View("Pelaporan/Index");
        }
        public IActionResult Penetapan()
        {
            return View("Penetapan/Index");
        }
        public IActionResult Pencarian()
        {
            return View("Pencarian/Index");
        }
    }
}
