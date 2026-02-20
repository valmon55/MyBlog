using Microsoft.AspNetCore.Mvc;

namespace KFA.MyBlog.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
