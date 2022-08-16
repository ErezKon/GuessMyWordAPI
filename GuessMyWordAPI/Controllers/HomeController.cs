using Microsoft.AspNetCore.Mvc;

namespace GuessMyWordAPI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
