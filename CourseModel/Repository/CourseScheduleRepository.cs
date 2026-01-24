using CourseData.Models;
using CourseService.Models;
using CourseService.Respository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseData.Repository
{
    public class CourseScheduleRepository : ICourseScheduleRepository
    {
        private readonly KhNetCourseContext _dbContext;
        public  CourseScheduleRepository(KhNetCourseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<CourseScheduleModel>> QueryAsync()
        {
            var query = from cs in _dbContext.Courseschedules
                        join t in _dbContext.Teachers on cs.Teacherid equals t.Id
                        join c in _dbContext.Courses on cs.Courseid equals c.Id
                        select new CourseScheduleModel
                        {
                            Id = cs.Id,
                            Code = c.Code,
                            Name = c.Name,
                            TeacherName = t.Name,
                            Des = c.Description,
                            Times = c.Times,
                            Sdate = cs.Sdate,
                            Edate = cs.Edate,
                            Location = cs.Location

                        };
            return await Task.FromResult(query.ToList());
        }
    }
}
