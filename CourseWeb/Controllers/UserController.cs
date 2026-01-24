using Microsoft.AspNetCore.Mvc;

namespace CourseWeb.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public IActionResult RegisterUser()
        {
            return View();
        }

        [HttpGet]
        
        public IActionResult Login()
        {
            return View();
        }
    }
}
