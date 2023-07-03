using Microsoft.AspNetCore.Mvc;

namespace cropsTrace.Controllers
{
    public class SeedInfoController : Controller
    {
        public IActionResult SeedInfo()
        {
            ViewData.Add("Action", "SeedInfo");
            return View("SeedInfo");
        }
    }
}
