using System.ComponentModel.DataAnnotations;

namespace CourseWeb.Models
{
    public class UserChgInfoViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Mobile { get; set; }
    }
}
