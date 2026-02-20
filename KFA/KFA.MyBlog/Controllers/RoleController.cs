using Microsoft.AspNetCore.Mvc;

namespace KFA.MyBlog.Controllers
{
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
