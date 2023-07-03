using Microsoft.AspNetCore.Mvc;

namespace cropsTrace.Controllers
{
    public class GrowthInfoController : Controller
    {
        public IActionResult GrowthInfo()
        {
            ViewData.Add("Action", "GrowthInfo");
            return View("GrowthInfo");
        }
    }
}
