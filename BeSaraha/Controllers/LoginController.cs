using Microsoft.AspNetCore.Mvc;

namespace BeSaraha.Controllers
{
    public class LoginController : Controller
    {
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
        public IActionResult Register(RegisterDTO register) 
        {
            //do stuff
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult Login(LoginDTO login)
        {
            // login
            throw new NotImplementedException();
        }
    }
}
