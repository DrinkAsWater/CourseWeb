using CourseService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseService.Interface
{
   public interface IUserCourseScheduleRepository
    {
        Task<bool> AddUserCouseScheduleAsync(Guid userId, Guid courseSceduleId);

        Task<bool> IsExistStudentCourseScheduleAsync(Guid userId, Guid courseSceduleId);

        Task<IEnumerable<ShopOrderModel>> FindStudentCourseSchedulesAsync(Guid userId);

        Task<bool> DeleteUserCourseScheduleAsync(Guid scheduleId);
       
    }
}
