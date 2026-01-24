using CourseService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseService.Interface
{
    public interface IUserRespository
    {
        Task<bool> CreateAsync(UserModel user);
        Task<UserModel> IsEmailExistsAsync(string email);

        Task<UserModel> FindByIdAsync(Guid id);

        Task<bool> UpdatePwdAsync(Guid id, string pwd);

        Task<bool> UpdateInfoAsync(UserInfoReqModel userInfoReqModel);
       
    }
}
