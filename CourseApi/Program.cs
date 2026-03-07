
using CourseData.Models;
using CourseData.Repository;
using CourseService.Interface;
using CourseService.Respository;
using CourseService.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace CourseApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<KhNetCourseContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("KhNetCourse")
    )
);
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;   // Cookie
            })
              .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, config =>
              {
                  config.Cookie.Name = "UserLoginCookie";
                  config.LoginPath = "/User/Login";
              });

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<ICourseScheduleService, CourseScheduleService>();
            builder.Services.AddScoped<ICourseScheduleRepository, CourseScheduleRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRespository, UserRepository>();
            builder.Services.AddScoped<IUserCourseScheduleRepository, UserCourseSceheduleRepository>();
            builder.Services.AddScoped<IShopService, ShopService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
