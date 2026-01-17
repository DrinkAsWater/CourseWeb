using CourseService.Interface;
using CourseService.Service;
using CourseWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CourseWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICourseScheduleService _courseScheduleService;

        public HomeController(ILogger<HomeController> logger, ICourseScheduleService courseScheduleService)
        {
            _logger = logger;
            _courseScheduleService = courseScheduleService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var vm = new List<CourseScheduleViewModel>();
            var model = await _courseScheduleService.QueryAsync();
            foreach (var item in model)
            {
                vm.Add(new CourseScheduleViewModel
                {
                    Id = item.Id,
                    CourseCode = item.Code,
                    CourseName = item.Name,
                    TeacherName = item.TeacherName,
                    Times = item.Times,
                    StartDate = item.Sdate,
                    EndDate = item.Edate,
                    Location = item.Location,
                    Desc = item.Des
                });
            }
            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
