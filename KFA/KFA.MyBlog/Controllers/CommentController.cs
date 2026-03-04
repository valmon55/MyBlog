using KFA.MyBlog.BLL.ViewModels.Comment;
using KFA.MyBlog.DAL.Entities;
using KFA.MyBlog.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KFA.MyBlog.Controllers
{
    public class CommentController : Controller
    {
        private readonly ILogger<CommentController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly ICommentService _commentService;

        public CommentController(UserManager<User> userManager,
                ILogger<CommentController> logger,
                ICommentService commentService)
        {
            _logger = logger;
            _userManager = userManager;
            _commentService = commentService;
        }

        [Route("AddComment")]
        [HttpGet]
        public IActionResult AddComment(int articleId)
        {
            //_logger.LogInformation($"Выполняется переход на страницу добавления комментария для статьи с ID = {articleId}");
            //return View(new CommentViewModel() { ArticleId = articleId} );
            return View(_commentService.AddComment(articleId));
        }
        [Route("AddComment")]
        [HttpPost]
        public async Task<IActionResult> AddComment(CommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                _commentService.AddComment(model, user);

                _logger.LogInformation($"Выполняется переход на страницу просмотра статьи c ID = {model.ArticleId}");

                return RedirectToAction("ViewArticle", "Article", new { Id = model.ArticleId });
            }
            else
            {
                _logger.LogError("Модель CommentViewModel при добавлении комментария невалидна!");
                ModelState.AddModelError("", "Ошибка в модели!");

                return View(model);
            }
        }
        [Route("AllArticleComments")]
        [HttpGet]
        public IActionResult AllArticleComments(int articleId)
        {
            return View(_commentService.AllArticleComments(articleId));
        }
        [Route("Comment/Delete")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var articleId = _commentService.DeleteComment(id);
            if (articleId is not null)
            {
                return RedirectToAction("ViewArticle", "Article", new { Id = articleId });
            }
            else
            {
                return RedirectToAction("AllArticles", "Article");
            }

        }
        [Route("Comment/Update")]
        [HttpGet]
        public IActionResult Update(int id)
        {
            return View("EditComment", _commentService.UpdateComment(id));
        }
        [Route("Comment/Update")]
        [HttpPost]
        public IActionResult Update(CommentViewModel model)
        {
            int articleId;

            if (ModelState.IsValid)
            {
                articleId = _commentService.UpdateComment(model);
                _logger.LogInformation($"Выполняется переход на страницу просмотра статьи c ID = {articleId.ToString()}");
                return RedirectToAction("ViewArticle", "Article", new { Id = articleId });
            }
            else
            {
                _logger.LogError("Модель CommentViewModel при обновлении комментария невалидна!");
                ModelState.AddModelError("", "Ошибка в модели!");
                _logger.LogWarning($"Выполняется переход на страницу просмотра всех статей.");
                return RedirectToAction("AllUserArticles", "Article");
            }
        }
    }
}
