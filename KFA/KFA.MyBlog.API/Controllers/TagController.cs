using KFA.MyBlog.API.Services.IServices;
using KFA.MyBlog.API.ViewModels.Tag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KFA.MyBlog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagController : Controller
    {
        private readonly ILogger<TagController> _logger;
        private readonly ITagService _tagService;
        public TagController(ILogger<TagController> logger,
                ITagService tagService)
        {
            _logger = logger;
            _tagService = tagService;
        }
        /// <summary>
        /// Добавление тега
        /// </summary>
        /// <remarks>
        /// Пример запроса
        /// POST
        /// {
        ///  "tag_Name": "string"
        /// }
        /// </remarks>
        /// <param name="model"> Данные для добавления тега </param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Moderator")]
        [Route("AddTag")]
        [HttpPost]
        public IActionResult AddTag(TagAddRequest model)
        {
            if (ModelState.IsValid)
            {
                _tagService.AddTag(model);
                return StatusCode(201);
            }
            else
            {
                _logger.LogError($"Ошибка в модели TagViewModel");
                ModelState.AddModelError("", "Ошибка в модели!");
                return StatusCode(403);
            }
        }
        /// <summary>
        /// Вывод списка всех тегов
        /// </summary>
        /// <returns></returns>
        [Route("AllTags")]
        [HttpGet]
        public List<TagRequest> AllTags()
        {
            return _tagService.AllTags();
        }
        /// <summary>
        /// Удаление тегов
        /// Доступно для пользователей с ролями Admin и Moderator
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Moderator")]
        [Route("DeleteTag")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                _tagService.DeleteTag(id);
                return StatusCode(201);
            }
            catch
            {
                return StatusCode(403);
            }
        }
        /// <summary>
        /// Обновление тегов
        /// Доступно для пользователей с ролями Admin и Moderator
        /// </summary>
        /// <remarks>
        /// Пример запроса
        /// POST
        /// {
        ///  "id": 0,
        ///  "tag_Name": "string"
        /// }
        /// </remarks>
        /// <param name="model"> Данные для обновления тега</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Moderator")]
        [Route("Update")]
        [HttpPost]
        public IActionResult Update(TagRequest model)
        {
            if (ModelState.IsValid)
            {
                _tagService.UpdateTag(model);
                return StatusCode(201);
            }
            else
            {
                _logger.LogError("Модель TagViewModel не прошла проверку!");
                ModelState.AddModelError("", "Ошибка в модели!");
                return StatusCode(500);
            }
        }
    }
}
