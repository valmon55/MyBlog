using AutoMapper;
using KFA.MyBlog.BLL.Extentions;
using KFA.MyBlog.BLL.ViewModels.Tag;
using KFA.MyBlog.Controllers;
using KFA.MyBlog.DAL.Entities;
using KFA.MyBlog.DAL.Repositories;
using KFA.MyBlog.DAL.UoW;
using KFA.MyBlog.Services.IServices;

namespace KFA.MyBlog.Services
{
    public class TagService : ITagService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<TagController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public TagService(IUnitOfWork unitOfWork,
                IMapper mapper,
                ILogger<TagController> logger)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public TagViewModel AddTag()
        {
            return new TagViewModel();
        }

        public void AddTag(TagViewModel model)
        {
            var tag = _mapper.Map<Tag>(model);
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            repo.Create(tag);
            _logger.LogInformation($"Создан тег {tag.Tag_Name}");
        }

        public List<TagViewModel> AllTags()
        {
            _logger.LogInformation($"Вывод списка всех тегов.");
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var tags = repo.GetAll();
            var tagsView = new List<TagViewModel>();
            foreach (var tag in tags)
            {
                tagsView.Add(_mapper.Map<TagViewModel>(tag));
            }
            return tagsView;
        }

        public void DeleteTag(int id)
        {
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            repo.DeleteTag(repo.GetTagById(id));
            _logger.LogInformation($"Удален тег с ID = {id}");
        }

        public TagViewModel UpdateTag(int id)
        {
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var tag = repo.GetTagById(id);
            var tagView = _mapper.Map<TagViewModel>(tag);
            _logger.LogInformation($"Тег для обновления: {tagView.Tag_Name}");

            return tagView;
        }

        public void UpdateTag(TagViewModel model)
        {
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var tag = repo.GetTagById(model.Id);
            tag.Convert(model);

            repo.UpdateTag(tag);
            _logger.LogInformation($"Тег {tag.Tag_Name} обновлен.");
        }
    }
}
