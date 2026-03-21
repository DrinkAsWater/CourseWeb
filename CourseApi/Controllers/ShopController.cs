using CourseApi.Request;
using CourseService.Interface;
using CourseService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;

        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShopOrderModel>>> GetShopOrder()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return BadRequest(new { errorCode = "E03", message = "使用者識別碼錯誤" });
            }
            var shopList = await _shopService.GetShopOrderListAsync(Guid.Parse(userId));
            return Ok(shopList);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ShopOrder(ShopCourseRequest shopCourseRequest)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return BadRequest(new { errorCode = "E02", message = "登記課程失敗(使用者使別碼不相符)" });
            }
            var result = await _shopService.AddShopOrderAsync(Guid.Parse(userId), shopCourseRequest.ScheduleId);
            if (!result)
            {
                return BadRequest(new { errorCode = "E01", message = "登記課失敗" });
            }
            return Ok(shopCourseRequest);
        }
        [Authorize]
        [HttpDelete("studentScheduleId")]
        public async Task<IActionResult> DeleteCourse(Guid studentScheduleId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new { errordCode = "E04", message = "使用者名稱未授權" });
            }

            var result = await _shopService.DeleteShopOrderAsync(studentScheduleId);
            if (!result)
            {
                return BadRequest(new { errordCode = "E06", message = "取消課程失敗" });
            }

            return NoContent();
        }
    }
}
