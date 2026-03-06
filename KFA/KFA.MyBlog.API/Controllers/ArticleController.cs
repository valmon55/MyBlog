using KFA.MyBlog.API.Services.IServices;
using KFA.MyBlog.API.ViewModels.Article;
using KFA.MyBlog.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KFA.MyBlog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        /// <summary>
        /// Создание статьи
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        /// POST
        /// {
        ///     "title": "string",
        ///     "articleDate": "2024-08-08T15:40:30.469Z",
        ///     "content": "string",
        ///     "tags": [
        ///         {
        ///             "id": 0,
        ///             "tag_Name": "string"
        ///         }
        ///     ]
        /// }
        /// </remarks>
        /// <param name="model">Содержит заголовок, текст и дату статьи</param>
        /// <returns></returns>
        [Authorize]
        [Route("AddArticle")]
        [HttpPost]
        public async Task<IActionResult> AddArticle(ArticleAddRequest model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                _articleService.AddArticle(model, user);

                return StatusCode(201);
            }
            else
            {
                _logger.LogError("Модель ArticleViewModel не прошла проверку!");

                ModelState.AddModelError("", "Ошибка в модели!");
                return StatusCode(403);
            }
        }
        /// <summary>
        /// Вывод всех статей автора
        /// </summary>
        /// <returns>Список статей</returns>
        [Authorize]
        [Route("AllUserArticles")]
        [HttpGet]
        public async Task<List<ArticleViewRequest>> AllUserArticles()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            return _articleService.AllArticles(user);
        }
        /// <summary>
        /// Вывод всех статей
        /// </summary>
        /// <returns>Список статей</returns>
        [Route("AllArticles")]
        [HttpGet]
        public List<ArticleViewRequest> AllArticles()
        {
            return _articleService.AllArticles();
        }
        /// <summary>
        /// Удаление статьи
        /// </summary>
        /// <param name="Id"> Id статьи</param>
        /// <returns></returns>
        [Authorize]
        [Route("Delete")]
        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            try
            {
                _articleService.DeleteArticle(Id);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(403, ex.Message);
            }
        }
        /// <summary>
        /// Редактирование статьи
        /// </summary>
        /// <remarks>
        /// POST
        /// {
        ///  "id": 0,
        ///  "title": "string",
        ///  "articleDate": "2024-08-09T09:04:57.392Z",
        ///  "content": "string",
        ///  "tags": [
        ///    {
        ///      "id": 0,
        ///      "tag_Name": "string"
        ///    }
        ///  ]
        /// }
        /// </remarks>>
        /// <param name="model">Данные со значениями статьи для обновления</param>
        /// <returns></returns>
        [Authorize]
        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(ArticleEditRequest model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                _articleService.UpdateArticle(model, user);

                return StatusCode(201);
            }
            else
            {
                _logger.LogError($"Ошибка в модели ArticleViewModel");
                ModelState.AddModelError("", "Ошибка в модели!");

                return StatusCode(403, "Неверные данные!");
            }
        }
    }
}
