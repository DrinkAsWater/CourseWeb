using CourseService.Interface;
using CourseService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseService.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRespository _userRespository;

        public UserService(IUserRespository userRespository)
        {
            _userRespository = userRespository;
        }
        public async Task<bool> UserRegister(UserModel user)
        {
            //驗證email是否重複
            var existUser = await _userRespository.IsEmailExistsAsync(user.Email);
            if (existUser != null) return false;

            //hash加密
            user.Id = Guid.NewGuid();
            user.Pwd = PasswrdHelper.PwdSHA256Hash(user.Pwd,user.Id.ToString());


            return await _userRespository.CreateAsync(user);
        }
    }
}
