using BeSaraha.DataAccess;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace BeSaraha.Controllers
{
    public class LoginController : Controller
    {
        private readonly BeSarahaDB _db;

        public LoginController(BeSarahaDB db)
        {
            _db = db;
        }
        public record RegisterDTO(string email,string firstname,string lastname,string password,bool keeplogin=false);
        public record LoginDTO(string email,string password,bool keeplogin=false);
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO register) 
        {
            //do stuff
            using var connection = _db.GetConnection();
            string? email = await connection.QueryFirstOrDefaultAsync<string>("SELECT email FROM Users WHERE email = @email",new {register.email});
            if (email == null)
            {
            await connection.QueryAsync("INSERT INTO Users (email,password,firstname,lastname) VALUES (@email,@password,@firstname,@lastname)", register);
            TempData["success"] = $"Welcome, {register.firstname} {register.lastname}!";
            return RedirectToAction("Index","Messages");
            }
            else
            {
                TempData["error"] = "That email already exists!";
                return RedirectToAction("Register", "Login");
            }
        }

        [HttpPost]
        public IActionResult Login(LoginDTO login)
        {
            // login
            throw new NotImplementedException();
        }
    }
}
