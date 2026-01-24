using CourseService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseService.Interface
{
    public interface IUserService
    {
        Task<bool> UserRegister(UserModel user);
    }
}
