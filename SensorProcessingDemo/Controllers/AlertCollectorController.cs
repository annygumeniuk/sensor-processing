using Microsoft.AspNetCore.Mvc;
using SensorProcessingDemo.Services.Interfaces;

namespace SensorProcessingDemo.Controllers
{
    public class AlertCollectorController : Controller
    {
        private readonly IAlertService _alertService;
        private readonly ICurrentUserService _currentUserService;

        public AlertCollectorController(IAlertService alertService, ICurrentUserService currentUserService)
        {
            _alertService = alertService;
            _currentUserService = currentUserService;
        }

        public async Task<IActionResult> Index()
        {
            int userId = Convert.ToInt32(_currentUserService.GetUserId());
            var alerts = await _alertService.GetAll(userId);

            return View(alerts);
        }
        
    }
}
