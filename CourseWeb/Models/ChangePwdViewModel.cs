using System.ComponentModel.DataAnnotations;

namespace CourseWeb.Models
{
    public class ChangePwdViewModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "原始密碼")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密碼長度必須在6到20個字元之間")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$",
           ErrorMessage = "密碼必須包含至少一個小寫字母、一個大寫字母和一個數字")]
        public string? OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "新密碼")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密碼長度必須在6到20個字元之間")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$",
       ErrorMessage = "密碼必須包含至少一個小寫字母、一個大寫字母和一個數字")]
        public string? Password { get; set; }


        [Required(ErrorMessage = "請確認密碼")]
        [DataType(DataType.Password)]
        [Display(Name = "確認新密碼")]
        [Compare("Password", ErrorMessage = "密碼與確認密碼不相符")]
        public string? ConfirmPwd { get; set; }
    }
}
