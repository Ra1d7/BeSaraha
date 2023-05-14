using BeSaraha.DataAccess;
using BeSaraha.Models;
using Dapper;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Plugins;
using System.Text.RegularExpressions;
using static BeSaraha.Controllers.UserSettings;

namespace BeSaraha.Helper
{
    public class UpdateUser
    {
        private string[] images = { ".jpg", ".png", ".jpeg", ".webp" };
        private readonly IWebHostEnvironment _env;
        private readonly BeSarahaDB _db;
        private readonly string nameregex = @"^[\p{L}\p{M}\p{N}\p{P}\p{Zs}]+$|[\p{Sc}]";

        public UpdateUser(IWebHostEnvironment env, BeSarahaDB db)
        {
            _env = env;
            _db = db;
        }
        public async Task<bool[]> updateUser(UserSettingsDTO user , string userid)
        {
            var dbuser = await GetUser(userid);
            bool picture_update = false;
            bool firstname_update = false;
            bool lastname_update = false;
            bool profile_url_update = false;
            if(user.picture != null)
            {
                picture_update = await updatePicture(user.picture, userid);
            }
            if(!user.firstname.IsNullOrEmpty() && user.firstname != dbuser.Firstname && Regex.IsMatch(user.firstname,nameregex))
            {
                firstname_update = await UpdateFirstName(user.firstname , userid);
            }
            if (!user.lastname.IsNullOrEmpty() && user.lastname != dbuser.Lastname && Regex.IsMatch(user.lastname, nameregex))
            {
                lastname_update = await UpdateLastName(user.lastname, userid);
            }
            if(user.profileurl != null && user.profileurl != dbuser.ProfileUrl && Regex.IsMatch(user.profileurl, nameregex))
            {
                profile_url_update = await UpdateProfileUrl(user.profileurl, userid);
            }
            bool[] checks = { picture_update , firstname_update , lastname_update , profile_url_update};
            if(checks.Any(b => b == true))
            {
                return new bool[]{true,profile_url_update};
            }
            return new bool[] {false,false};
        }

        private async Task<bool> UpdateProfileUrl(string profileurl, string userid)
        {
            using var connection = _db.GetConnection();
            int rows = await connection.ExecuteAsync("UPDATE USERS SET profileurl = @profileurl WHERE id = @userid", new { profileurl, userid });
            if (rows == 1) return true;
            return false;
        }

        private async Task<bool> UpdateLastName(string lastname, string userid)
        {
            using var connection = _db.GetConnection();
            int rows = await connection.ExecuteAsync("UPDATE USERS SET lastname = @lastname WHERE id = @userid", new { lastname, userid });
            if (rows == 1) return true;
            return false;
        }

        private async Task<bool> UpdateFirstName(string firstname, string userid)
        {
            using var connection = _db.GetConnection();
            int rows = await connection.ExecuteAsync("UPDATE USERS SET firstname = @firstname WHERE id = @userid",new { firstname , userid });
            if (rows == 1) return true;
            return false;
        }

        private async Task<bool> updatePicture(IFormFile picture , string userid)
        { 
            using var connection1 = _db.GetConnection();
            string oldProfileUrl = await connection1.QueryFirstAsync<string>("SELECT picture FROM USERS WHERE id = @userid", new { userid });
            string oldPath = Path.Combine(_env.WebRootPath, "assets", "profile_pictures", oldProfileUrl);
            File.Delete(oldPath);
            string filename = picture.FileName;
            var stream = picture.OpenReadStream();
            string filePath = Path.Combine(_env.WebRootPath, "assets", "profile_pictures", filename);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await picture.CopyToAsync(fileStream);
                using var connection = _db.GetConnection();
                int rows = await connection.ExecuteAsync("UPDATE USERS SET picture = @filename WHERE id = @userid", new { filename, userid });

                if (rows == 1)
                {
                    return true;
                }
            }
            return false;
        }

        private async Task<User> GetUser(string userid)
        {
            using var connection = _db.GetConnection();
            User dbuser = await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE id = @userid", new { userid });
            return dbuser;
        }
    }
}
