using AutoMapper;
using KFA.MyBlog.BLL.Extentions;
using KFA.MyBlog.BLL.ViewModels.Comment;
using KFA.MyBlog.Controllers;
using KFA.MyBlog.DAL.Entities;
using KFA.MyBlog.DAL.Repositories;
using KFA.MyBlog.DAL.UoW;
using KFA.MyBlog.Services.IServices;

namespace KFA.MyBlog.Services
{
    public class CommentService : ICommentService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ArticleController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(ILogger<ArticleController> logger,
                IUnitOfWork unitOfWork,
                IMapper mapper
            )
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public CommentViewModel AddComment(int articleId)
        {
            _logger.LogInformation($"Выполняется переход на страницу добавления комментария для статьи с ID = {articleId}");
            return new CommentViewModel() { ArticleId = articleId };
        }

        public void AddComment(CommentViewModel model, User user)
        {
            var comment = _mapper.Map<Comment>(model);

            comment.CommentDate = DateTime.Now;
            comment.User = user;
            comment.UserId = comment.User.Id;
            var repo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
            repo.Create(comment);
            _logger.LogInformation($"Комментарий создал пользователь {comment.User.UserName} : {comment.User.First_Name} {comment.User.Last_Name}");
        }

        public List<CommentViewModel> AllArticleComments(int articleId)
        {
            _logger.LogInformation($"Выполняется переход на страницу просмотра всех статей комментариев статьи с ID = {articleId}.");
            var repo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
            var comments = repo.GetComments();
            var commentsView = new List<CommentViewModel>();
            foreach (var comment in comments)
            {
                if (comment.ArticleId == articleId)
                {
                    commentsView.Add(_mapper.Map<CommentViewModel>(comment));
                }
            }
            return commentsView;
        }

        public int? DeleteComment(int id)
        {
            var repo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
            var comment = repo.GetCommentById(id);
            _logger.LogInformation($"Удаление комментария с ID = {id}");

            repo.Delete(comment);

            return comment.ArticleId;
        }

        public CommentViewModel UpdateComment(int id)
        {
            var repo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
            var comment = repo.GetCommentById(id);
            var commentView = _mapper.Map<CommentViewModel>(comment);
            _logger.LogInformation($"Выполняется переход на страницу обновления комментария с ID = {id}. ID статьи = {comment.ArticleId}.");

            return commentView;
        }

        public int UpdateComment(CommentViewModel model)
        {
            var repo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
            var comment = repo.GetCommentById(model.Id);
            comment.Convert(model);

            repo.Update(comment);

            return comment.ArticleId;
        }
    }
}
