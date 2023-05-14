using BeSaraha.DataAccess;
using BeSaraha.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace BeSaraha.Controllers
{
    public class UserSettings : Controller
    {
        private readonly BeSarahaDB _db;
        public record UserSettingsDTO(string firstname,string lastname,string profileurl,IFormFile? picture = null);

        public UserSettings(BeSarahaDB db)
        {
            _db = db;
        }
        public async Task<IActionResult> Settings()
        {
            string userid = HttpContext.User.Claims.ToArray()[2].Value;
            using var connection = _db.GetConnection();
            var user = await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE id = @userid", new { userid });
            var userDto = new UserSettingsDTO(user.Firstname,user.Lastname,user.ProfileUrl);
            return View(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> SaveSettings(UserSettingsDTO user)
        {
            if(ModelState.IsValid)
            {
                TempData["success"] = "Your profile has been updated!";
            }
            else
            {
                TempData["error"] = "An error has occured while saving changes";
            }
            return RedirectToAction("Settings", "UserSettings");
        }
    }
}
