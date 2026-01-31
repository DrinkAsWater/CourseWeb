using CourseService.Interface;
using CourseService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseWeb.Controllers
{
    public class ShopController : Controller
    {
        private readonly IShopService _shopService;

    
        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }
        [Authorize]
        [HttpGet]
        public async Task <IActionResult> Index()
        {
            var shopList = await _shopService.GetShopOrderListAsync(Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var vm  = new List<UserCourseScheduleViewModel>();
            foreach (var item in shopList)
            {
                vm.Add(new UserCourseScheduleViewModel
                {
                    Id = item.Id,
                    CourseName = item.CourseName,
                    TeacherName = item.TeacherName,
                    CourseDate = $"{item.CourseStartDate}~{item.CourseEndDate}"
                });
            }
            return View(vm);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShopOrder(Guid scheduleid)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _shopService.AddShopOrderAsync(Guid.Parse(currentUserId), scheduleid);
            TempData["ShopOrderMessage"] = result ? "登記" : "重複登記課程";

            return RedirectToAction("index", "home");

        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>DeleteShopOrder(Guid userScheduleId)
        {
            var result = await _shopService.DeleteShopOrderAsync(userScheduleId);
            TempData["ShopOrderMessage"] = result ? "刪除成功":"刪除失敗";
            return RedirectToAction("index");

        }

    }
}
