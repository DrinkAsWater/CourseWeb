
using CourseApi.Middleware;
using CourseApi.security;
using CourseData.Models;
using CourseData.Repository;
using CourseService.Interface;
using CourseService.Respository;
using CourseService.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

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
            builder.Services.AddScoped<IJwtHelper,JwtHelper>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //取得配置 JWT 服務
            var jwtSettings = builder.Configuration.GetSection("JwtTokenSettings");

            /*
               DefaultAuthenticateScheme: 指定當系統需要識別User時使用的認證機制
               當請求進入應用程序並需要確定「誰在發送這個請求」時，系統會使用這個方案來解析請求中的認證資料
               （在 JWT 的情況下，就是從 HTTP 請求頭中的 Authorization 字段提取並驗證 Bearer token）

               DefaultChallengeScheme: 指定當User需要登錄時使用的認證機制
               在 JWT 認證中，通常是返回 401 Unauthorized 狀態碼，並可能包含一個 WWW-Authenticate 頭，指導client端如何提供有效的憑證
            */
            builder.Services
                .AddAuthentication(
                    options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    }
                ) //新增驗證服務
                .AddJwtBearer(
                    options =>
                    {
                        //允許在 HTTP 和 HTTPS 連接上接受 JWT 令牌，不會檢查連接是否安全(HTTPS)(通常是開發環境才會關閉)
                        options.RequireHttpsMetadata = false;

                        //jwt token 驗證參數
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true, //驗證發行者
                            ValidateAudience = true, //驗證受眾
                            ValidateLifetime = true, //驗證存活期
                            RequireExpirationTime = true,  // 明確要求JWT必須包含過期時間
                            ValidateIssuerSigningKey = true, //驗證簽名金鑰
                            ValidIssuer = jwtSettings["Issuer"],
                            ValidAudience = jwtSettings["Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["IssuerSigningKey"])), //對稱簽章金鑰
                            ClockSkew = TimeSpan.Zero // 不允許時鐘偏移
                        };

                        //建立一個新的 JwtBearerEvents 實例，用於處理 JWT 認證過程中的各種事件
                        options.Events = new JwtBearerEvents();

                        //JWT檢驗失敗的回應處理
                        options.Events.OnChallenge = context =>
                        {
                            // 當驗證失敗時，回應標頭包含 WWW-Authenticate 標頭，顯示失敗的詳細原因
                            // 生產環境中通常應該設為 false，因為這涉及安全性考量
                            options.IncludeErrorDetails = false;
                            context.HandleResponse();
                            context.Response.ContentType = "application/json";
                            context.Response.StatusCode = 401;

                            var responseContent = new
                            {
                                error = "Unauthorized",
                                error_description = "Authentication failed" // 生產環境不應暴露詳細錯誤資訊
                            };

                            return context.Response.WriteAsync(JsonSerializer.Serialize(responseContent));
                        };

                    }
                ); //使用 JWT 進行驗證

            var app = builder.Build();
            // 註冊全域例外處理中介軟體 (必須放在最前面)
            app.UseMiddleware<GlobalExceptionMiddleware>();

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
