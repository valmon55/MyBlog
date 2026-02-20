using KFA.MyBlog.DAL.Configs;
using KFA.MyBlog.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFA.MyBlog.DAL
{
    public class MyBlogContext : IdentityDbContext<User>
    {
        override public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public MyBlogContext(DbContextOptions<MyBlogContext> options) : base(options)
        {
            Database.EnsureCreated();
            //Database.EnsureDeleted();
            //Database.Migrate();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration<Article>(new ArticleConfiguration());
            builder.ApplyConfiguration<Comment>(new CommentConfiguration());
            builder.ApplyConfiguration<Tag>(new TagConfiguration());
            //builder.ApplyConfiguration<Article_Tags>(new Article_TagsConfiguration());
        }
    }
}
