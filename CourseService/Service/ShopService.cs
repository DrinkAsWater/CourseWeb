using CourseService.Interface;
using CourseService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseService.Service
{
    public class ShopService : IShopService
    {
        private readonly IUserCourseScheduleRepository _userCourseScheduleRepository;
        public ShopService(IUserCourseScheduleRepository userCourseScheduleRepository)
        {
            _userCourseScheduleRepository = userCourseScheduleRepository;
        }

     
        public async Task<bool> AddShopOrderAsync(Guid userId, Guid courseSceduleId)
        {
            var result = await _userCourseScheduleRepository.IsExistStudentCourseScheduleAsync(userId, courseSceduleId);
            if (result)
            {
                return false;
            }

            return await _userCourseScheduleRepository.AddUserCouseScheduleAsync(userId, courseSceduleId);
        }

        public async Task<bool> DeleteShopOrderAsync(Guid scheduleId)
        {
            return await _userCourseScheduleRepository.DeleteUserCourseScheduleAsync(scheduleId);
        }

        public  async Task<IEnumerable<ShopOrderModel>> GetShopOrderListAsync(Guid userId)
        {
            return await _userCourseScheduleRepository.FindStudentCourseSchedulesAsync(userId);
        }
    }
}
