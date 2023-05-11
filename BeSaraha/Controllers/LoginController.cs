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
        public record RegisterDTO(string email, string firstname, string lastname, string password, bool keeplogin = false);
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
                using var connection = _db.GetConnection();
                string? email = await connection.QueryFirstOrDefaultAsync<string>("SELECT email FROM Users WHERE email = @email", new { register.email });
                if (email == null)
                {
                    await connection.QueryAsync("INSERT INTO Users (email,password,firstname,lastname) VALUES (@email,@password,@firstname,@lastname)", register);
                    TempData["success"] = $"Welcome, {register.firstname} {register.lastname}!";
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
                var result = await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE email = @email AND password = @password", new {  login.email, login.password });
                if (result != null)
                {
                    List<Claim> claims = new() { new(ClaimTypes.NameIdentifier, login.email) };
                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        IsPersistent = login.keeplogin
                    };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity), properties);
                    TempData["success"] = $"Logged in as {login.email}";
                }
            }
            TempData["error"] = "Invalid Credentials!";
            return View();
        }
    }
}
