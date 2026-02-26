using CourseService.Interface;
using CourseService.Models;
using CourseService.Respository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseService.Service
{
    public class CourseScheduleService : ICourseScheduleService
    {
        private readonly ICourseScheduleRepository _courseScheduleRepository;
        public CourseScheduleService(ICourseScheduleRepository courseScheduleRepository) {
            _courseScheduleRepository = courseScheduleRepository;
        }
        public async Task<IEnumerable<CourseScheduleModel>> QueryAsync()
        {
            return await _courseScheduleRepository.QueryAsync();
        }

        public async Task<IEnumerable<CourseScheduleModel>> QueryAsync(string? keyword)
        {
            return await _courseScheduleRepository.QueryAsync(keyword);
        }
    }
}
