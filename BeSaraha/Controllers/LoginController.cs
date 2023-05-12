using BeSaraha.DataAccess;
using BeSaraha.Models;
using Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BeSaraha.Controllers
{
    public class LoginController : Controller
    {
        private readonly BeSarahaDB _db;

        public LoginController(BeSarahaDB db)
        {
            _db = db;
        }
        public record RegisterDTO(string email, string firstname, string lastname, string password, bool keeplogin = false, string profileUrl = "");
        public record LoginDTO(string email, string password, bool keeplogin = false);
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Messages");
            }
            return View();
        }
        public IActionResult Register()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Messages");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            if (ModelState.IsValid)
            {
                int exists = -1;
                using var connection = _db.GetConnection();
                string? email = await connection.QueryFirstOrDefaultAsync<string>("SELECT email FROM Users WHERE email = @email", new { register.email });
                if (register.profileUrl != "") { exists = await connection.QueryFirstOrDefaultAsync<int>("SELECT ProfileUrl from USERS WHERE ProfileUrl = @profileUrl", new { register.profileUrl }); }
                if(exists != 0) { TempData["error"] = "Url is already used :("; return View(); }
                if (email == null)
                {
                    string profileUrl = "";
                    int? result = -1;
                    while (result != 0)
                    {
                        if (register.profileUrl == "") profileUrl = Random.Shared.Next(10000, 100000).ToString();
                        profileUrl = register.profileUrl;
                        result = await connection.QueryFirstOrDefaultAsync<int>("SELECT ProfileUrl from USERS WHERE ProfileUrl = @profileUrl", new { profileUrl });
                    }
                    await connection.QueryAsync("INSERT INTO Users (email,password,firstname,lastname,profileurl) VALUES (@email,@password,@firstname,@lastname,@profileurl)", new { register.email, register.password, register.firstname, register.lastname, profileUrl });
                    TempData["success"] = $"Successfully registered, please login!";
                    return RedirectToAction("Index", "Messages");
                }
                else
                {
                    TempData["error"] = "That email already exists!";
                    return RedirectToAction("Register", "Login");
                }
            }
            TempData["error"] = "please recheck the data youve entered";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            if (ModelState.IsValid)
            {
                using var connection = _db.GetConnection();
                var result = await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE email = @email AND password = @password", new { login.email, login.password });
                if (result != null)
                {
                    List<Claim> claims = new() { new(ClaimTypes.Name, $"{result.Firstname} {result.Lastname}") };
                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        IsPersistent = login.keeplogin
                    };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity), properties);
                    TempData["success"] = $"Successfully loggedin!";
                    return RedirectToAction("Index", "Messages");
                }
                else
                {
                    TempData["error"] = "Invalid Credentials!";
                }
            }
            return View();
        }
    }
}
