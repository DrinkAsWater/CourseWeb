using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseService.Models
{
    public class UserInfoReqModel
    {
        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string Mobile {  get; set; }
    }
}
