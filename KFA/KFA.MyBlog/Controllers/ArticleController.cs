using KFA.MyBlog.BLL.ViewModels.Article;
using KFA.MyBlog.DAL.Entities;
using KFA.MyBlog.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KFA.MyBlog.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ILogger<ArticleController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IArticleService _articleService;

        public ArticleController(ILogger<ArticleController> logger,
                UserManager<User> userManager,
                IArticleService articleService
            )
        {
            _logger = logger;
            _userManager = userManager;
            _articleService = articleService;
        }
        [Authorize]
        [Route("AddArticle")]
        [HttpGet]
        public async Task<IActionResult> AddArticle()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            return View(_articleService.AddArticle(user));
        }
        [Authorize]
        [Route("AddArticle")]
        [HttpPost]
        public async Task<IActionResult> AddArticle(ArticleViewModel model, List<int> SelectedTags)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                _articleService.AddArticle(model, SelectedTags, user);

                return RedirectToAction("AllUserArticles");
            }
            else
            {
                _logger.LogError("Модель ArticleViewModel не прошла проверку!");

                ModelState.AddModelError("", "Ошибка в модели!");
                return RedirectToAction("AllUserArticles");
            }
        }
        [Authorize]
        [Route("AllUserArticles")]
        [HttpGet]
        public async Task<IActionResult> AllUserArticles()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            return View("AllArticles", _articleService.AllArticles(user));
        }

        [Route("AllArticles")]
        [HttpGet]
        public IActionResult AllArticles()
        {
            return View("AllArticles", _articleService.AllArticles());
        }
        [Route("ViewArticle")]
        [HttpGet]
        public IActionResult ViewArticle(int Id)
        {
            return View("Article", _articleService.ViewArticle(Id));
        }

        [Authorize]
        [Route("Delete")]
        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            _articleService.DeleteArticle(Id);

            return RedirectToAction("AllUserArticles", "Article");
        }

        [Authorize]
        [Route("Article/Update")]
        [HttpGet]
        public async Task<IActionResult> Update(int Id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            return View("EditArticle", _articleService.UpdateArticle(Id, user));
        }

        [Authorize]
        [Route("Article/Update")]
        [HttpPost]
        public async Task<IActionResult> Update(ArticleViewModel model, List<int> SelectedTags)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                _articleService.UpdateArticle(model, SelectedTags, user);

                return RedirectToAction("AllUserArticles", "Article");
            }
            else
            {
                _logger.LogError($"Ошибка в модели ArticleViewModel");
                ModelState.AddModelError("", "Ошибка в модели!");

                return RedirectToAction("AllUserArticles", "Article");
            }
        }
    }
}
