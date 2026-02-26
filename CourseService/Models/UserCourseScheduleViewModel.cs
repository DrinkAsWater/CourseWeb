using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseService.Models
{
   public class UserCourseScheduleViewModel
    {
      public  Guid Id { get; set; }

        public string CourseName { get; set; }

        public string TeacherName {  get; set; }

        public string CourseDate { get; set; }
    }
}
