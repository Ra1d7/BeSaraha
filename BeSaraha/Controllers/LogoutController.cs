using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BeSaraha.Controllers
{
    public class LogoutController : Controller
    {
        public async Task<IActionResult> Index()
        {
            await HttpContext.SignOutAsync();
            TempData["success"] = TempData["success"];
            return RedirectToAction("Index","Messages");
        }
    }
}
