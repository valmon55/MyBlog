using KFA.MyBlog.API.Services.IServices;
using KFA.MyBlog.API.ViewModels.Comment;
using KFA.MyBlog.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KFA.MyBlog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        /// <summary>
        /// Добавление комментария к статье
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// POST
        /// {
        ///  "comment": "string",
        ///  "articleId": 0
        /// }
        /// </remarks>
        /// <param name="model">Данные комментария для добавления</param>
        /// <returns></returns>
        [Authorize]
        [Route("AddComment")]
        [HttpPost]
        public async Task<IActionResult> AddComment(CommentAddRequest model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                _commentService.AddComment(model, user);
                return StatusCode(201);
            }
            else
            {
                _logger.LogError("Модель CommentViewModel при добавлении комментария невалидна!");
                return StatusCode(403);
            }
        }
        /// <summary>
        /// Вывод всех комментарие к статье
        /// </summary>
        /// <param name="articleId"> Id статьи </param>
        /// <returns> Список комментариев </returns>
        [Route("AllArticleComments")]
        [HttpGet]
        public List<CommentViewRequest> AllArticleComments(int articleId)
        {
            return _commentService.AllArticleComments(articleId);
        }
        /// <summary>
        /// Удаление комментария
        /// </summary>
        /// <param name="id"> Id комментария</param>
        /// <returns></returns>
        [Authorize]
        [Route("Delete")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var articleId = _commentService.DeleteComment(id);
            if (articleId is not null)
            {
                return StatusCode(201);
            }
            else
            {
                return StatusCode(403);
            }
        }
        /// <summary>
        /// Обновление комментария
        /// </summary>
        /// <remarks>
        /// Пример запроса
        /// POST
        /// {
        ///   "id": 0,
        ///   "comment": "string"
        /// }
        /// </remarks>
        /// <param name="model"> Данные для обновления комментария </param>
        /// <returns></returns>
        [Authorize]
        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(CommentEditRequest model)
        {
            int articleId;

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByNameAsync(User.Identity.Name);
                    articleId = _commentService.UpdateComment(model, user);
                    _logger.LogInformation($"Выполняется переход на страницу просмотра статьи c ID = {articleId}");
                    return StatusCode(201);
                }
                catch (Exception ex)
                {
                    return Problem(ex.Message);
                }
            }
            else
            {
                _logger.LogError("Модель CommentViewModel при обновлении комментария невалидна!");
                _logger.LogWarning($"Выполняется переход на страницу просмотра всех статей.");
                return StatusCode(403);
            }
        }
    }
}
