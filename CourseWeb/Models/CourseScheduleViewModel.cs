namespace CourseWeb.Models
{
    public class CourseScheduleViewModel
    {
        public Guid Id { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string TeacherName { get; set; }

        public int Times { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public string Location {  get; set; }

        public string Desc { get; set; }


    }
}
