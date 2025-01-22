using Microsoft.AspNetCore.Mvc;
using SensorProcessingDemo.Repositories.Implementations;
using SensorProcessingDemo.Services;
using System.Diagnostics;
using SensorProcessingDemo.Models;
using SensorProcessingDemo.Repositories.Interfaces;

namespace SensorProcessingDemo.Controllers
{
    public class AccountController : Controller
    {
        private readonly IEntityRepository<User> _userRepo;

        public AccountController(IEntityRepository<User> entityRepository)
        {
            _userRepo = entityRepository;
        }

        public IActionResult Index()
        {         
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User newUser)
        {
            if (!ModelState.IsValid)
            {
                return View(newUser); // Return the view with validation errors
            }
            
            await _userRepo.AddAsync(newUser); // Add user to repository
            await _userRepo.SaveAsync(); // Save changes to database

            return RedirectToAction("Index");
        }
    }
}
