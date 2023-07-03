using Microsoft.AspNetCore.Mvc;

namespace cropsTrace.Controllers
{
    public class MobileController : Controller
    {
        public IActionResult MobilePage(
            string companyId="",
            string year="",
            string cropsId="",
            string pumpHouseID=""
            )
        {
            ViewData.Add("companyId",companyId);
            ViewData.Add("year", year);
            ViewData.Add("cropsId", cropsId);
            ViewData.Add("pumpHouseID", pumpHouseID);
            return View("MobilePage");
        }
    }
}
