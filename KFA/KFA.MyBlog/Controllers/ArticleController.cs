using Microsoft.AspNetCore.Mvc;

namespace KFA.MyBlog.Controllers
{
    public class ArticleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
