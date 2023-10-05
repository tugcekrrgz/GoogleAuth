using Microsoft.AspNetCore.Mvc;

namespace GoogleAuth.MVC.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
    }
}
