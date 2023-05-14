using BeSaraha.DataAccess;
using BeSaraha.Models;
using Dapper;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;

namespace BeSaraha.Controllers
{
    public class UserSettings : Controller
    {
        private string[] images = { ".jpg", ".png", ".jpeg", ".webp" };
        private readonly BeSarahaDB _db;
        private readonly IWebHostEnvironment _env;

        public record UserSettingsDTO(string firstname,string lastname,string profileurl,IFormFile? picture = null);

        public UserSettings(BeSarahaDB db,IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
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
             string filename = user.picture.FileName;
            if (ModelState.IsValid && images.Contains(Path.GetExtension(filename)))
            {
                var stream = user.picture.OpenReadStream();
                string filePath = Path.Combine(_env.WebRootPath, "assets", filename);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await user.picture.CopyToAsync(fileStream);
                    using var connection = _db.GetConnection();
                    string userid = HttpContext.User.Claims.ToArray()[2].Value;
                    int rows = await connection.ExecuteAsync("UPDATE USERS SET picture = @filename WHERE id = @userid", new { filename, userid });
                    if (rows == 1)
                    {
                        TempData["success"] = "Your profile has been updated!";
                    }
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
