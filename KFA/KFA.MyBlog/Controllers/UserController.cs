using Microsoft.AspNetCore.Mvc;

namespace KFA.MyBlog.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
