using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseService.Models
{
    public class UserPwdReqModel
    {
        public Guid UserId { get; set; }
        public string NewPwd { get; set; }
        public string OldPwd { get; set; }
    }
}

