using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseService.Models
{
   public class ShopOrderModel
    {
        public Guid Id { get; set; }

        public string CourseName { get; set; }

        public string TeacherName { get; set; }

        public DateOnly CourseStartDate { get; set; }

        public DateOnly CourseEndDate { get; set; } // 修正拼字

        public int Times { get; set; }
    }
}
