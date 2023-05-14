using BeSaraha.DataAccess;
using BeSaraha.Helper;
using BeSaraha.Models;
using Dapper;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;

namespace BeSaraha.Controllers
{
    public class UserSettings : Controller
    {
        private readonly BeSarahaDB _db;
        private readonly UpdateUser _updateusr;
        private readonly IWebHostEnvironment _env;

        public record UserSettingsDTO(string firstname,string lastname,string profileurl,IFormFile? picture = null);

        public UserSettings(BeSarahaDB db, UpdateUser updateusr)
        {
            _db = db;
            _updateusr = updateusr;
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
            if (ModelState.IsValid)
            {
                string currentUserId = HttpContext.User.Claims.ToArray()[2].Value;
                bool[] result = await _updateusr.updateUser(user, currentUserId);
                if (result[0]) TempData["success"] = "Your profile has been updated!";
                if (result[1])
                {
                TempData["success"] = "You need to relog to see changes";
                return RedirectToAction("Index", "Logout");
                }
            }
            else
            {
                TempData["error"] = "An error has occured while saving changes";
            } 
                return RedirectToAction("Settings", "UserSettings");
        }
    }
}
