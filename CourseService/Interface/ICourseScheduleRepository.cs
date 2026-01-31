using CourseService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseService.Respository
{
   public interface ICourseScheduleRepository
    {
        Task<IEnumerable<CourseScheduleModel>> QueryAsync();

        Task<IEnumerable<CourseScheduleModel>> QueryAsync(string? keyword);
    }
}
