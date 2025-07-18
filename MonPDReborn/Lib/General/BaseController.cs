using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;


namespace MonPDReborn.Lib.General
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            // Cek login sebelum menjalankan setiap action di turunan controller
            if (!HttpContext.IsUserLoggedIn())
            {
                TempData["ERROR_LOGIN"] = "Session habis, silahkan login kembali";
                context.Result = new RedirectToActionResult("Index", "Login", null);
            }
        }
    }
}
