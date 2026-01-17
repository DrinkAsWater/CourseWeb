using System.ComponentModel.DataAnnotations;

namespace CourseWeb.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "請輸入帳號")]
        [Display(Name ="帳號")]
        public string ? UserName { get; set; }

        [Required(ErrorMessage = "請輸入密碼")]
        [Display(Name = "密碼")]
        public string? Password { get; set; }



    }
}
