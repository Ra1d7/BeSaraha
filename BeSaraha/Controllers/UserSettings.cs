using Microsoft.AspNetCore.Mvc;

namespace BeSaraha.Controllers
{
    public class UserSettings : Controller
    {
        public IActionResult Settings()
        {
            return View();
        }
    }
}
