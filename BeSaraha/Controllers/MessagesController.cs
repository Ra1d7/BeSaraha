using BeSaraha.DataAccess;
using BeSaraha.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BeSaraha.Controllers
{
    public class MessagesController : Controller
    {
        private readonly ILogger<MessagesController> _logger;
        private readonly BeSarahaDB _db;

        public MessagesController(ILogger<MessagesController> logger,BeSarahaDB db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Login");
            }
            string userid = HttpContext.User.Claims.Where(x => x.Type == "userid").FirstOrDefault().Value;
            using var connection = _db.GetConnection();
            var messages = await connection.QueryAsync<Message>("SELECT * FROM Messages WHERE userid = @userid", new { userid });
            return View(messages.ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}