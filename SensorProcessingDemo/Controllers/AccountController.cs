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
            var allUsers = _userRepo.GetAllAsync();            
            return View();
        }      
    }
}
