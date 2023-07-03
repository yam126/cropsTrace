using Microsoft.AspNetCore.Mvc;

namespace cropsTrace.Controllers
{
    /// <summary>
    /// 泵房管理页面
    /// </summary>
    public class PumpHouseController : Controller
    {
        public IActionResult PumpHouse()
        {
            ViewData.Add("Action", "PumpHouse");
            return View("PumpHouse");
        }
    }
}
