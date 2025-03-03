using Microsoft.AspNetCore.Mvc;
using SensorProcessingDemo.Models;
using SensorProcessingDemo.Repositories.Interfaces;
using SensorProcessingDemo.Auth;

namespace SensorProcessingDemo.Controllers
{
    public class AccountController : Controller
    {
        private readonly IEntityRepository<User> _userRepo;
        private readonly IJwtProvider _jwtProvider;

        public AccountController(IEntityRepository<User> entityRepository,
            IJwtProvider jwtProvider)
        {
            _userRepo = entityRepository;
            _jwtProvider = jwtProvider;
        }

        private void Authenticate(User user)
        {
            var token = _jwtProvider.GenerateToken(user);
            HttpContext.Response.Cookies.Append("tasty-cookies", token);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User newUser)
        {
            if (!ModelState.IsValid)
            {
                return View(newUser);
            }
            
            await _userRepo.AddAsync(newUser);            

            Authenticate(newUser);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {                        
            var user = (await _userRepo.
                FindAsync(u => u.Email == email && u.Password == password)).
                FirstOrDefault();

            if (user != null)
            {
                Authenticate(user);

                return RedirectToAction("Index", "Home");
            }
            return BadRequest();
        }

        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("tasty-cookies");
            return RedirectToAction("Login", "Account");
        }
    }
}