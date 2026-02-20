using Microsoft.AspNetCore.Mvc;

namespace KFA.MyBlog.Controllers
{
    public class ErrorController : Controller
    {
        [Route("error")]
        [HttpGet]
        public IActionResult Error(int code)
        {
            if (code == 404)
            {
                return View("ResourceNotFound");
            }
            else if (code == 403)
            {
                return View("AccessRestricted");
            }
            else
            {
                return View("SomethingGoesWrong");
            }
        }
    }
}
