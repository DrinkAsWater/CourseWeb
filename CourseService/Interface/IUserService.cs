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
        Task<bool> UserRegisterAsync(UserModel user);

        Task<UserModel> UserSignAsync(string email, string pwd);

        Task<bool>UserPwdUpdateAsync(UserPwdReqModel userPwdReqModel);

        Task<UserModel> FindUserAsync(Guid UserId);

        Task<bool>UserInfoUpdateAsync(UserInfoReqModel userInfoReqModel);

     
    }
}
