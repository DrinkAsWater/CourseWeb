using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppClient.Models
{
    public class CourseScheduleModel
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string TeacherName { get; set; }

        public string Desc { get; set; } = string.Empty;

        public int Times { get; set; }

        public DateOnly Sdate { get; set; }

        public DateOnly Edate { get; set; }

        public string Location { get; set; } = string.Empty;
    }
}
