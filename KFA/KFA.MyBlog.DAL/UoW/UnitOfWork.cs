using KFA.MyBlog.DAL.Repositories;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFA.MyBlog.DAL.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyBlogContext _blogContext;
        private Dictionary<Type, object> _repositories;

        public UnitOfWork(MyBlogContext blogContext)
        {
            _blogContext = blogContext;
        }
        public void Dispose()
        {

        }
        public IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = true) where TEntity : class
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<Type, object>();
            }
            if (hasCustomRepository)
            {
                var customRepository = _blogContext.GetService<IRepository<TEntity>>();
                if (customRepository != null)
                {
                    return customRepository;
                }
            }
            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new Repository<TEntity>(_blogContext);
            }
            return (IRepository<TEntity>)_repositories[type];
        }
        public int SaveChanges(bool ensureAutoHistory = false)
        {
            throw new NotImplementedException();
        }
    }
}
