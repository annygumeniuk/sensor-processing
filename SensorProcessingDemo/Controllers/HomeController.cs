using Microsoft.AspNetCore.Mvc;

namespace SensorProcessingDemo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
