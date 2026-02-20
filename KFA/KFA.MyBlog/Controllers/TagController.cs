using Microsoft.AspNetCore.Mvc;

namespace KFA.MyBlog.Controllers
{
    public class TagController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
