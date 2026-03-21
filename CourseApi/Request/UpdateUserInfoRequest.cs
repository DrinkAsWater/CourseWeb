using System.ComponentModel.DataAnnotations;

namespace CourseApi.Request
{
    public class UpdateUserInfoRequest
    {
        [Required]
        public string Name { get; set; }
        public string? Mobile { get; set; }
    }
}