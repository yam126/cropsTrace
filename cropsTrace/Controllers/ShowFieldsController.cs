using Microsoft.AspNetCore.Mvc;

namespace cropsTrace.Controllers
{
    public class ShowFieldsController : Controller
    {
        public IActionResult ShowFields()
        {
            ViewData.Add("Action", "ShowFields");
            return View("ShowFields");
        }
    }
}
