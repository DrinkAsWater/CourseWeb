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

        public async Task<IEnumerable<CourseScheduleModel>> QueryAsync(string? keyword)
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

            // 如果有 keyword 就加條件
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.ToLower(); // 忽略大小寫
                query = query.Where(x =>
                    x.Name.ToLower().Contains(keyword) ||
                    x.Code.ToLower().Contains(keyword) ||
                    x.TeacherName.ToLower().Contains(keyword));
            }

            return await Task.FromResult(query.ToList());
        }

    }
}
