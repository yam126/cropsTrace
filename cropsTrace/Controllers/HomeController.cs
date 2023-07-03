using Microsoft.AspNetCore.Mvc;

namespace cropsTrace.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData.Add("Action", "Home");
            return View("Index");
        }
    }
}
