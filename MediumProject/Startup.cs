using MediumProject.EF_MediumProject_CodeFirst.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediumProject.EF_MediumProject_CodeFirst.Repository.Abstract;
using MediumProject.EF_MediumProject_CodeFirst.Repository.Concrete;
using MediumProject.Settings;
using MediumProject.Models.Model_Repository.Abstract;
using MediumProject.Models.Model_Repository.Concrete;

namespace MediumProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<MediumContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("SqlConnectionString")));

            services.AddMvc();
            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddSession();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IStoryRepository, StoryRepository>();
            services.AddScoped<ITopicRepository, TopicRepository>();

            services.AddScoped<ITopicVMRepository, TopicVMRepository>();
            services.AddScoped<IUserVMRepository, UserVMRepository>();
            services.AddScoped<IStoryVMRepository, StoryVMRepository>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddTransient<IEmailRepository, EmailRepository>();

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSession();
            app.UseRouting();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
