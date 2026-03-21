using CourseApi.Request;
using CourseApi.security;
using CourseService.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace CourseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtHelper _jwtHelper;
        public AuthController(IUserService userService, JwtHelper jwtHelper)
        {
            _userService = userService;
            _jwtHelper = jwtHelper;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            //登入
            var member = await _userService.UserSignAsync(request.Username, request.Password);
            if (member == null)
            {
                return Unauthorized(new { message = "login fail" });
            }
            //生成jwt Token
            var response = _jwtHelper.GenerateToken(member);

            return Ok(response);


        }
    }
}
