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
    }
}
