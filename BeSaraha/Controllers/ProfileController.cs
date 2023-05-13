using BeSaraha.DataAccess;
using BeSaraha.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace BeSaraha.Controllers
{
    public class ProfileController : Controller
    {
        public record UserAndMessage(User user, Message message);
        private readonly BeSarahaDB _db;

        public ProfileController(BeSarahaDB db)
        {
            _db = db;
        }
        [HttpGet]
        [Route("/Profile/")]
        [Route("/Profile/{url}")]
        public async Task<IActionResult> Index([FromRoute] string url = "moath17")
        {
            using var connection = _db.GetConnection();
            var user = await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE profileurl=@url", new { url });
            if (user == null)
            {
                TempData["error"] = "No profile was found";
                return RedirectToAction("Index", "Messages");
            }
            UserAndMessage model = new UserAndMessage(user, new Message());
            return View(model);
        }
        [HttpPost]
        [Route("/Profile/")]
        [Route("/Profile/{url}")]
        public async Task<IActionResult> SendMessage(int userid)
        {
            string e = HttpContext.Request.Form["msgtext"];
            if (e.Length > 0 && e.Length < 1000)
            {
                Message msg = new Message() { Date = DateTime.Now, UserId = userid, Text = e };
                using var connection = _db.GetConnection();
                int rows = await connection.ExecuteAsync("INSERT INTO Messages (Userid,text,date) VALUES (@userid,@text,@date)", msg);
                if(rows == 1)
                {
                    await connection.ExecuteAsync("UPDATE Users SET messagescount = messagescount +1 WHERE id = @userid", new { userid });
                TempData["success"] = "Message has been sent!";
                }
                else
                {
                    TempData["error"] = "An error has occured while sending the message";
                }
            }
            else
            {
                TempData["error"] = "that is not a valid message!";
            }
            
            return RedirectToAction("Index", "Profile");
        }
    }
}
