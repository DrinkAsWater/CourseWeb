using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseService.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Pwd { get; set; }

        public string Email { get; set; }

        public string? Mobile { get; set; }

        // 新增欄位支援 OAuth / Google 登入
        public int Provider { get; set; }    
        // 0=本地帳號, 1=Google
        public string? ProviderUserId { get; set; } // Google 唯一識別 (sub)
    }
}

   
