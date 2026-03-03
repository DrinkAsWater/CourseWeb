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

        public async Task<UserModel> FindUserAsync(Guid UserId)
        {
            return await _userRespository.FindByIdAsync(UserId); 
        }

        public async Task<UserModel> OAuthLoginAsync(string providerUserId, string name, string email)
        {
            // 1️⃣ 找是否已有 Google 綁定
            var user = await _userRespository.FindByProviderAsync(1, providerUserId);
            if (user != null) return user;

            // 2️ Google ID 未綁定 → 查本地 Email
            var localUser = await _userRespository.IsEmailExistsAsync(email);
            if (localUser != null)
            {
                // 原本這兩行物件賦值 + UpdateInfoAsync 都刪掉
                await _userRespository.BindProviderAsync(localUser.Id, 1, providerUserId);
                return localUser;
            }

            //新帳號
            var newUser = new UserModel
            {
                Id = Guid.NewGuid(),
                UserName = name,
                Email = email,
                Pwd = null,
                Provider = 1,
                ProviderUserId = providerUserId
            };
            await _userRespository.CreateAsync(newUser);
            return newUser;
        }

        public async Task<bool> UserInfoUpdateAsync(UserInfoReqModel userInfoReqModel)
        {
           await _userRespository.UpdateInfoAsync(userInfoReqModel);
            return true;

        }

        public async Task<bool> UserPwdUpdateAsync(UserPwdReqModel userPwdReqModel)
        {
            //1.檢查帳號是否存在
            var existUser = await _userRespository.FindByIdAsync(userPwdReqModel.UserId);
            if (existUser == null) return false;
            //驗證舊密碼
            var oldHashPwd = PasswordHelper.PwdSHA256Hash(userPwdReqModel.OldPwd,userPwdReqModel.UserId.ToString());
            if (oldHashPwd != existUser.Pwd) return false;
            //更新密碼
            var newHashPwd = PasswordHelper.PwdSHA256Hash(userPwdReqModel.NewPwd,userPwdReqModel.UserId.ToString());

            return await _userRespository.UpdatePwdAsync(userPwdReqModel.UserId, newHashPwd);

            }
        public async Task<bool> UserRegisterAsync(UserModel user)
        {
            //驗證email是否重複
            var existUser = await _userRespository.IsEmailExistsAsync(user.Email);
            if (existUser != null) return false;

            //hash加密
            user.Id = Guid.NewGuid();
            user.Pwd = PasswordHelper.PwdSHA256Hash(user.Pwd,user.Id.ToString());


            return await _userRespository.CreateAsync(user);
        }

        public async Task<bool> UserSetPwdAsync(Guid userId, string newPwd)
        {
            var hashPwd = PasswordHelper.PwdSHA256Hash(newPwd, userId.ToString());
            return await _userRespository.UpdatePwdAsync(userId, hashPwd);
        }

        public async Task<UserModel> UserSignAsync(string email, string pwd)
        {
            //檢查帳號是否存在
            var existUser = await _userRespository.IsEmailExistsAsync(email);
            if (existUser == null) return null;
            //檢查是否正確
            var hashPwd = PasswordHelper.PwdSHA256Hash(pwd,existUser.Id.ToString());
            if (hashPwd != existUser.Pwd) return null;

            return existUser;
        }
    }
}
