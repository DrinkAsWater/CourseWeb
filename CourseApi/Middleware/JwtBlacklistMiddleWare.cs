using CourseApi.security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CourseApi.Middleware
{
    public class JwtBlacklistMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ITokenBlacklistService _blacklistService;
        public JwtBlacklistMiddleWare(RequestDelegate next, ITokenBlacklistService blacklistService)
        {
            _next = next;
            _blacklistService = blacklistService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var jti = context.User.FindFirstValue(JwtRegisteredClaimNames.Jti);
            if(context.User.Identity?.IsAuthenticated == true)
            {
            if (!string.IsNullOrEmpty(jti) && _blacklistService.IsBlacklisted(jti))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
                {
                    error = "Unauthorized",
                    error_description = "token has been revoked"

                })

                    );
                return;
            }

            }
            await _next(context);
        }

        }
    }

