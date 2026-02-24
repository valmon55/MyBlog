using KFA.MyBlog.DAL.Configs;
using KFA.MyBlog.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace KFA.MyBlog.DAL
{
    public class MyBlogContext : IdentityDbContext<User>
    {
        public override DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public MyBlogContext(DbContextOptions<MyBlogContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
            //Database.Migrate();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                //entity.HasKey(ur => new { ur.UserId, ur.RoleId });

                // Настройка связи с пользователем
                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(ur => ur.UserId)
                    .OnDelete(DeleteBehavior.Restrict); // Изменяем с Cascade на Restrict

                // Настройка связи с ролью
                entity.HasOne<UserRole>()
                    .WithMany()
                    .HasForeignKey(ur => ur.RoleId)
                    .OnDelete(DeleteBehavior.Restrict); // Изменяем с Cascade на Restrict
            });

            builder.Entity<Article>(entity =>
            {
                //entity.HasKey(c => new { c.Id });

                entity.HasOne<User>(u => u.User)
                    .WithMany(a => a.Articles)
                    .HasForeignKey(c => c.UserId)
                    //.HasConstraintName("FK_Comments_AspNetUsers_UserId")
                    .OnDelete(DeleteBehavior.Restrict); // Изменяем с Cascade на Restrict
            });

            builder.Entity<Comment>(entity =>
            {
                entity.HasOne<Article>(a => a.Article)
                    .WithMany(c => c.Comments)
                    .HasForeignKey(c => c.ArticleId)
                    //.HasConstraintName("FK_Comments_AspNetUsers_UserId")
                    .OnDelete(DeleteBehavior.Restrict); // Изменяем с Cascade на Restrict
            });
            builder.ApplyConfiguration<Article>(new ArticleConfiguration());
            builder.ApplyConfiguration<Comment>(new CommentConfiguration());
            builder.ApplyConfiguration<Tag>(new TagConfiguration());
        }
    }
}
