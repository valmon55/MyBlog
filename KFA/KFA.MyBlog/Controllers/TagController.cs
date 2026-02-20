using AutoMapper;
using KFA.MyBlog.BLL.ViewModels.Tag;
using KFA.MyBlog.DAL.Entities;
using KFA.MyBlog.DAL.Repositories;
using KFA.MyBlog.DAL.UoW;
using KFA.MyBlog.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace KFA.MyBlog.Controllers
{
    public class TagController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<TagController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITagService _tagService;
        public TagController(IUnitOfWork unitOfWork,
                IMapper mapper,
                ILogger<TagController> logger,
                ITagService tagService)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _tagService = tagService;
        }
        [Route("AddTag")]
        [HttpGet]
        public IActionResult AddTag()
        {
            return View(_tagService.AddTag());
        }
        [Route("AddTag")]
        [HttpPost]
        public IActionResult AddTag(TagViewModel model)
        {
            if (ModelState.IsValid)
            {
                _tagService.AddTag(model);
            }
            else
            {
                _logger.LogError($"Ошибка в модели TagViewModel");
                ModelState.AddModelError("", "Ошибка в модели!");
            }
            return RedirectToAction("AllTags", "Tag");
        }
        [Route("AllTags")]
        [HttpGet]
        public IActionResult AllTags()
        {
            return View(_tagService.AllTags());
        }
        [Route("Tag")]
        [HttpGet]
        public IActionResult TagById(int id)
        {
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var tag = repo.GetTagById(id);
            var tagView = _mapper.Map<TagViewModel>(tag);

            //пока неизвестно где буду использовать
            return RedirectToAction("AllTags", "Tag");
        }
        [Route("DeleteTag")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _tagService.DeleteTag(id);

            return RedirectToAction("AllTags", "Tag");
        }
        [Route("Tag/Update")]
        [HttpGet]
        public IActionResult Update(int id)
        {
            return View("EditTag", _tagService.UpdateTag(id));
        }
        [Route("Tag/Update")]
        [HttpPost]
        public IActionResult Update(TagViewModel model)
        {
            if (ModelState.IsValid)
            {
                _tagService.UpdateTag(model);
            }
            else
            {
                _logger.LogError("Модель TagViewModel не прошла проверку!");
                ModelState.AddModelError("", "Ошибка в модели!");
            }

            _logger.LogInformation($"Перенаправление на страницу просмотра всех тегов.");
            return RedirectToAction("AllTags", "Tag");
        }
    }
}
