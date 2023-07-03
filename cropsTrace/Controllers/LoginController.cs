using Microsoft.AspNetCore.Mvc;

namespace cropsTrace.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
