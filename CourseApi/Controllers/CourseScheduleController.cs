using CourseApi.Response;
using CourseService.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CourseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseScheduleController : ControllerBase
    {
        private readonly ICourseScheduleService _courseScheduleService;

        public CourseScheduleController(ICourseScheduleService courseScheduleService)
        {
           _courseScheduleService = courseScheduleService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseScheduleResponse>>> GetCourse()
        {
            var courses = await _courseScheduleService.QueryAsync();
            var response = courses.Select(c => new CourseScheduleResponse
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name,
                TeacherName = c.TeacherName,
                Desc = c.Des,
                Times = c.Times,
                Date = c.Sdate.ToShortDateString() + "~" + c.Edate.ToShortDateString(),
                Location = c.Location

            });

            return Ok(response);    
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CourseScheduleResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CourseScheduleResponse>> GetCourseById(Guid id)
        {
            var course = await _courseScheduleService.QueryAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            var response = new CourseScheduleResponse
            {
                Id = course.Id,
                Code = course.Code,
                Name = course.Name,
                TeacherName = course.TeacherName,
                Desc = course.Des,
                Times = course.Times,
                Date = course.Sdate.ToShortDateString() + "~" + course.Edate.ToShortDateString(),
                Location = course.Location

            };
               return Ok(response);
        }
        [AllowAnonymous]
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<CourseScheduleResponse>),StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CourseScheduleResponse>>> SearchCourse([FromQuery] string? keyword)
        {
            var course = await _courseScheduleService.QueryAsync(keyword);
            var response = course.Select(c => new CourseScheduleResponse
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name,
                TeacherName = c.TeacherName,
                Desc = c.Des,
                Times = c.Times,
                Date = c.Sdate.ToShortDateString() + "~" + c.Edate.ToShortDateString(),
                Location = c.Location

            });
            return Ok(response);
        }
       
    }
}
