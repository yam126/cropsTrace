using Microsoft.AspNetCore.Mvc;

namespace cropsTrace.Controllers
{
    public class CompanyInfoController : Controller
    {
        public IActionResult CompanyInfo()
        {
            ViewData["Action"] = "CompanyInfo";
            return View();
        }
    }
}
