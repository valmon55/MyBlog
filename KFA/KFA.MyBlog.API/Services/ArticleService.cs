using KFA.MyBlog.API.Controllers;
using KFA.MyBlog.DAL.Entities;
using KFA.MyBlog.DAL.Repositories;
using KFA.MyBlog.DAL.UoW;
using KFA.MyBlog.API.Extentions;
using KFA.MyBlog.API.Services.IServices;
using KFA.MyBlog.API.ViewModels.Article;
using KFA.MyBlog.API.ViewModels.Tag;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KFA.MyBlog.API.Services
{
    public class ArticleService : IArticleService
    {
        private readonly ILogger<TagController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Article> _articleRepository;
        private readonly IRepository<Tag> _tagRepository;
        private readonly UserManager<User> _userManager;
        public ArticleService(IUnitOfWork unitOfWork,
                ILogger<TagController> logger,
                IRepository<Article> articleRepository,
                IRepository<Tag> tagRepository,
                UserManager<User> userManager
                )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _articleRepository = articleRepository;
            _tagRepository = tagRepository;
            _userManager = userManager;
        }

        public int AddArticle(ArticleAddRequest model, User user)
        {
            _logger.LogInformation($"Создаёт статью пользователь {user.UserName} : {user.First_Name} {user.Last_Name}");

            var dbTags = new List<Tag>();

            if(model.Tags != null)
            {
                var tagsId = model.Tags.Select(t => t.Id).ToList();
                dbTags = _tagRepository.GetAll().Where(t => tagsId.Contains(t.Id)).ToList();
            }

            var article = new Article()
            {
                ArticleDate = DateTime.Now,
                Title = model.Title,
                Content = model.Content,
                User = user,
                Tags = dbTags
            };

            _logger.LogInformation("Выполняется добавление новой статьи статьи.");
            _articleRepository.Create(article);

            return article.Id;
        }

        public List<ArticleViewRequest> AllArticles(User user = null)
        {
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var articles = new List<Article>();
            var articlesView = new List<ArticleViewRequest>();

            if (user is not null)
            {
                articles = repo.GetArticlesByUserId(user.Id);
            }
            else
            {
                articles = repo.GetArticles();
            }

            articlesView = articles.Select(p => new ArticleViewRequest()
            {
                Id = p.Id,
                ArticleDate = p.ArticleDate,
                AuthorId = p.UserId,
                Title = p.Title,
                Content = p.Content,
                Tags = p.Tags.Select(t => new TagRequest() { Id = t.Id, Tag_Name = t.Tag_Name }).ToList()
            }).ToList();

            return articlesView;
        }

        public List<ArticleViewRequest> AllUserArticles()
        {
            throw new NotImplementedException();
        }

        public void DeleteArticle(int id)
        {
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;

            repo.DeleteArticle(repo.Get(id));
        }

        public async void UpdateArticle(ArticleEditRequest model, User user)
        {
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var article = repo.GetArticleById(model.Id);
            if (article == null)
                return;
            // Если пользователь не админ, не модератор и если это не его статья
            // то изменять статью нельзя
            if (!(article.User.Id == user.Id ||
                    _userManager.IsInRoleAsync(user, "Admin").Result ||
                    _userManager.IsInRoleAsync(user, "Admin").Result))
                return;

            var tagRepo = _unitOfWork.GetRepository<Tag>() as TagRepository;

            var dbTags = tagRepo.GetAll().ToList();
            
            // Проверяем ID тегов из запроса
            // убираем те, которых нет
            var checkedModelTagsId = model.Tags.Select(x => x.Id).Intersect(dbTags.Select(x => x.Id)).ToList();
            
            if (checkedModelTagsId.Count > 0)
            {
                var addTagsId = checkedModelTagsId.Except(article.Tags.Select(x => x.Id)).ToList();
                var delTagsId = article.Tags.Select(x => x.Id).Except(checkedModelTagsId).ToList();

                foreach (var dbTag in dbTags)
                {
                    if (addTagsId.Contains(dbTag.Id))
                    {
                        article.Tags.Add(dbTag);
                    }
                    if (delTagsId.Contains(dbTag.Id))
                    {
                        article.Tags.Remove(dbTag);
                    }
                }
            }
        
            article.User = user;
            article.ArticleDate = model.ArticleDate;
            article.Title = model.Title;
            article.Content = model.Content;

            _logger.LogInformation($"Обновление статьи:\n" + $"дата {article.ArticleDate.ToShortDateString()} {article.ArticleDate.ToShortTimeString()} \n" +
                    $"заголовок {article.Title} \n" + $"текст {article.Content}");

            repo.Update(article);
        }
    }
}
