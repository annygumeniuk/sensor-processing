using Microsoft.AspNetCore.Mvc;
using SensorProcessingDemo.Repositories.Implementations;
using SensorProcessingDemo.Services;
using System.Diagnostics;
using SensorProcessingDemo.Models;
using SensorProcessingDemo.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
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

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };

            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }


        public IActionResult Index(string? firstName)
        {
            ViewBag.FirstName = !string.IsNullOrWhiteSpace(firstName) ? firstName : "Login or register";
            return View();
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
            await _userRepo.SaveAsync();

            await Authenticate(newUser.Email);

            return RedirectToAction("Index");
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
            var user = (await _userRepo.FindAsync(u => u.Email == email && u.Password == password)).FirstOrDefault();
            if (user != null)
            {
                var token = _jwtProvider.GenerateToken(user);

                HttpContext.Response.Cookies.Append("tasty-cookies", token);

                return View("Index");
            }
            else 
            {
                throw new Exception();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
