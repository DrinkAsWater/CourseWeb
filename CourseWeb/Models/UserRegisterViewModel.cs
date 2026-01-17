using System.ComponentModel.DataAnnotations;

public class UserRegisterViewModel
{
    [Required(ErrorMessage = "請輸入使用者名稱")]
    [Display(Name = "使用者名稱")]
    [StringLength(50, ErrorMessage = "使用者名稱長度不可超過50個字元")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "請輸入密碼")]
    [DataType(DataType.Password)]
    [Display(Name = "密碼")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "密碼長度必須在6到20個字元之間")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$",
        ErrorMessage = "密碼必須包含至少一個小寫字母、一個大寫字母和一個數字")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "請確認密碼")]
    [DataType(DataType.Password)]
    [Display(Name = "確認密碼")]
    [Compare("Password", ErrorMessage = "密碼與確認密碼不相符")]
    public string? ConfirmPwd { get; set; }

    [Required(ErrorMessage = "請輸入電子郵件")]
    [EmailAddress(ErrorMessage = "請輸入有效的電子郵件格式")]
    [Display(Name = "電子郵件(登入帳號)")]
    public string? Email { get; set; }
}
