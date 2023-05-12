using BeSaraha.DataAccess;
using BeSaraha.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace BeSaraha.Controllers
{
    public class ProfileController : Controller
    {
        private readonly BeSarahaDB _db;

        public ProfileController(BeSarahaDB db)
        {
            _db = db;
        }
        [HttpGet]
        [Route("/Profile/")]
        [Route("/Profile/{url}")]
        public async Task<IActionResult> Index([FromRoute]string url="moath17")
        {
            using var connection = _db.GetConnection();
            var user = await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE profileurl=@url",new {url});
            if (user == null)
            {
                TempData["error"] = "No profile was found";
                return RedirectToAction("Index", "Messages");
            }
            return View(user);
        }
    }
}
