using Microsoft.AspNetCore.Mvc;

namespace SensorProcessingDemo.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
