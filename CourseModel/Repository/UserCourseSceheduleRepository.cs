using CourseData.Models;
using CourseService.Interface;
using CourseService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseData.Repository
{
    public class UserCourseSceheduleRepository : IUserCourseScheduleRepository
    {
        private readonly KhNetCourseContext _dbContext;
        public UserCourseSceheduleRepository(KhNetCourseContext dbcontext)
        {
            _dbContext = dbcontext;
        }
        public async Task<bool> AddUserCouseScheduleAsync(Guid userId, Guid courseSceduleId)
        {
            await _dbContext.Stucourseschedules.AddAsync(new Stucourseschedule
            {
                Id = Guid.NewGuid(),
                Studentid = userId,
                Cscheduleid = courseSceduleId
            });
            await _dbContext.SaveChangesAsync();

            return true;
        }
        public async Task<bool> IsExistStudentCourseScheduleAsync(Guid userId, Guid courseSceduleId)
        {
            return await _dbContext.Stucourseschedules
                 .AnyAsync(x => x.Studentid == userId && x.Cscheduleid == courseSceduleId);
        }

        public async Task<IEnumerable<ShopOrderModel>> FindStudentCourseSchedulesAsync(Guid userId)
        {
            var query = from stusc in _dbContext.Stucourseschedules
                        join cs in _dbContext.Courseschedules on stusc.Cscheduleid equals cs.Id
                        join c in _dbContext.Courses on cs.Courseid equals c.Id
                        join t in _dbContext.Teachers on cs.Teacherid equals t.Id
                        where stusc.Studentid == userId
                        select new ShopOrderModel
                        {
                            Id = stusc.Id,
                            CourseName = c.Name,
                            TeacherName = t.Name,
                            CourseStartDate = cs.Sdate,
                           CourseEndDate = cs.Edate,
                            Times = c.Times
                        };
            return await query.ToListAsync();
        }

        public async Task<bool> DeleteUserCourseScheduleAsync(Guid scheduleId)
        {
            var entity = await _dbContext.Stucourseschedules.FirstOrDefaultAsync(x => x.Id == scheduleId);
            if (entity == null)
            {
                return false;
            }
            _dbContext.Stucourseschedules.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
