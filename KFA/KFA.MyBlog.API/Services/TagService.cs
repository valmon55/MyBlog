using KFA.MyBlog.API.Controllers;
using KFA.MyBlog.DAL.Entities;
using KFA.MyBlog.DAL.Repositories;
using KFA.MyBlog.DAL.UoW;
using KFA.MyBlog.API.Extentions;
using KFA.MyBlog.API.ViewModels.Tag;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace KFA.MyBlog.API.Services.IServices
{
    public class TagService : ITagService
    {
        private readonly ILogger<TagController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public TagService(IUnitOfWork unitOfWork,
                ILogger<TagController> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public void AddTag(TagAddRequest model)
        {
            var tag = new Tag() { Tag_Name = model.Tag_Name };
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            repo.Create(tag);
            _logger.LogInformation($"Создан тег {tag.Tag_Name}");
        }
        public List<TagRequest> AllTags()
        {
            _logger.LogInformation($"Вывод списка всех тегов.");
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var tags = repo.GetAll();
            var tagsView = new List<TagRequest>();
            foreach (var tag in tags)
            {
                //tagsView.Add(_mapper.Map<TagViewModel>(tag));
                tagsView.Add(new TagRequest() { Id = tag.Id, Tag_Name = tag.Tag_Name});
            }
            return tagsView;
        }
        public void DeleteTag(int id)
        {
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            repo.DeleteTag(repo.GetTagById(id));
            _logger.LogInformation($"Удален тег с ID = {id}");
        }
        public void UpdateTag(TagRequest model)
        {
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var tag = repo.GetTagById(model.Id);
            //tag.Convert(model);
            tag.Tag_Name = model.Tag_Name;

            repo.UpdateTag(tag);
            _logger.LogInformation($"Тег {tag.Tag_Name} обновлен.");
        }
    }
}
