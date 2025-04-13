using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SensorProcessingDemo.Services.Interfaces;
using AnalyticalUnit.Utils;

namespace SensorProcessingDemo.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserService _userService;

        public AdminController(ICurrentUserService currentUserService, IUserService userService)
        {
            _currentUserService = currentUserService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int userId = Convert.ToInt32(_currentUserService.GetUserId());
            var adminInfo = await _userService.GetUser(userId);

            return View(adminInfo);
        }

        [HttpGet]
        public async Task<IActionResult> AllUsers()
        {
            int userId = Convert.ToInt32(_currentUserService.GetUserId());
            var allUsers = await _userService.GetAll(userId);

            return View(allUsers);
        }
        
        [HttpPost]
        public async Task<IActionResult> ChangeUserRole(int userId)
        {
            await _userService.ChangeUserRole(userId);
            return RedirectToAction("AllUsers");                        
        }

        [HttpPost]
        public async Task<IActionResult> UploadCsv(IFormFile file)
        {
            var forecaster = new WeatherForecaster();

            // Generate forecast for next 24 hours
            var forecastResult = forecaster.ForecastWeather(file, 24);

            return Redirect("Forecasting");
        }

        [HttpGet]
        public async Task<IActionResult> Forecasting()
        {            
            return View();
        }        
    }
}
