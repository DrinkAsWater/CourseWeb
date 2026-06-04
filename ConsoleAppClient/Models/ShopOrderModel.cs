using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppClient.Models
{
    public class ShopOrderModel
    {
        public Guid Id { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public DateOnly CourseStartDate { get; set; }
        public DateOnly CourseEndDate { get; set; }
        public int Times { get; set; }
    }

    public class CreateOrderRequest
    {
        public Guid StuId { get; set; }
        public Guid ScheduleId { get; set; }
    }
}

