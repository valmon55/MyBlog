using KFA.MyBlog.DAL;
using KFA.MyBlog.DAL.Entities;
using KFA.MyBlog.DAL.Extentions;
using KFA.MyBlog.DAL.Repositories;
using KFA.MyBlog.API.Services;
using KFA.MyBlog.API.Services.IServices;
using KFA.MyBlog.API.Validators;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace KFA.MyBlog.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("HP_Connection");
            var mapperConfig = new MapperConfiguration(v =>
            {
                v.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ArticleViewModelValidator>())
                .AddDbContext<MyBlogContext>(options => options.UseSqlServer(connection))
                .AddUnitOfWork()
                .AddCustomRepository<Article, ArticleRepository>()
                .AddTransient<IArticleService, ArticleService>()
                .AddCustomRepository<Comment, CommentRepository>()
                .AddTransient<ICommentService, CommentService>()
                .AddCustomRepository<Tag, TagRepository>()
                .AddTransient<ITagService, TagService>()
                .AddCustomRepository<User, UserRepository>()
                .AddTransient<IUserService, UserService>()
                .AddCustomRepository<UserRole, UserRoleRepository>()
                .AddTransient<IRoleService, RoleService>()
                .AddIdentity<User, UserRole>(opts =>
                {
                    opts.Password.RequiredLength = 6;
                    opts.Password.RequireNonAlphanumeric = false;
                    opts.Password.RequireDigit = false;
                })
                .AddEntityFrameworkStores<MyBlogContext>()
            ;
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                var baseDir = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(baseDir, "KFA.MyBlog.API.xml");

                c.IncludeXmlComments(xmlPath);

                c.SwaggerDoc("v2", new OpenApiInfo 
                { 
                    Version = "v 2.1",
                    Title = "KFA.MyBlog.API", 
                    Contact = new OpenApiContact() { Email = "fedor_ka@mail.ru", Name = "Fedor K.A.", 
                        Url = new System.Uri("https://www.blog.com") }
                });
            });
            //Подключение куки
            services.AddAuthentication(optionts => optionts.DefaultScheme = "Cookies")
                .AddCookie("Cookies", options =>
                {
                    options.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = redirectContext =>
                        {
                            // Если пользоватепль не прошел аунтефекацию - выводим ошибку 401
                            redirectContext.HttpContext.Response.StatusCode = 401;
                            return Task.CompletedTask;
                        }
                    };
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v2/swagger.json", "KFA.MyBlog.API v1"));
            }

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
