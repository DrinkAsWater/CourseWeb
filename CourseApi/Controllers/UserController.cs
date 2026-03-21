using CourseApi.Request;
using CourseService.Interface;
using CourseService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("info")]
        public async Task<IActionResult> GetInfo()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var member = await _userService.FindUserAsync(userId);

            if (member == null)
                return NotFound(new { message = "找不到使用者" });

            return Ok(new
            {
                member.UserName,
                member.Mobile,
                member.Email
            });
        }

        [HttpPut("info")]
        public async Task<IActionResult> UpdateInfo([FromBody] UpdateUserInfoRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _userService.UserInfoUpdateAsync(new UserInfoReqModel
            {
                UserId = userId,
                Name = request.Name,
                Mobile = request.Mobile
            });

            if (!result)
                return BadRequest(new { message = "更新失敗" });

            return Ok(new { message = "更新成功" });
        }
    }
}