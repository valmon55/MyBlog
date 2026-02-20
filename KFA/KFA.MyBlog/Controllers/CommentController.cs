using Microsoft.AspNetCore.Mvc;

namespace KFA.MyBlog.Controllers
{
    public class CommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
