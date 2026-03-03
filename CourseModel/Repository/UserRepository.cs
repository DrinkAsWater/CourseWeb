using CourseData.Models;
using CourseService.Interface;
using CourseService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseData.Repository
{
 
    public class UserRepository : IUserRespository
    {
        private readonly KhNetCourseContext _dbContext;
        public UserRepository(KhNetCourseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task BindProviderAsync(Guid userId, int provider, string providerUserId)
        {
            var user = await _dbContext.Students.FindAsync(userId);
            if (user == null) return;

            user.Provider = provider;
            user.ProviderUserId = providerUserId;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> CreateAsync(UserModel user)
        {
            await _dbContext.AddAsync(new Student()
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Password = user.Pwd,
                Provider = user.Provider,
                ProviderUserId = user.ProviderUserId
            });

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<UserModel> FindByIdAsync(Guid id)
        {
            UserModel userModel = null;
            var student = await _dbContext.Students.FirstOrDefaultAsync(s => s.Id == id);
            if (student != null)
            {
                userModel = new UserModel()
                {
                    Id = student.Id,
                    UserName = student.Name ?? string.Empty,
                    Email = student.Email ?? string.Empty,
                    Pwd = student.Password ?? string.Empty,
                    Mobile = student.Mobile ?? string.Empty,
                };
            }
            return userModel;
        }

        public async Task<UserModel> FindByProviderAsync(int provider, string providerUserId)
        {
            var student = await _dbContext.Students
        .FirstOrDefaultAsync(s => s.Provider == provider && s.ProviderUserId == providerUserId);

            if (student == null) return null;

            return new UserModel
            {
                Id = student.Id,
                UserName = student.Name ?? string.Empty,
                Email = student.Email ?? string.Empty,
                Pwd = student.Password ?? string.Empty,
                Mobile = student.Mobile ?? string.Empty,
                Provider = student.Provider,
                ProviderUserId = student.ProviderUserId
            };
        }

        public async Task<UserModel> IsEmailExistsAsync(string email)
        {
            UserModel userModel = null;
            var student = await _dbContext.Students.FirstOrDefaultAsync(s => s.Email == email);
            if (student != null)
            {
                userModel = new UserModel()
                {
                    Id = student.Id,
                    UserName = student.Name ?? string.Empty,
                    Email = student.Email ?? string.Empty,
                    Pwd = student.Password ?? string.Empty,


                };
            }
            return userModel;
        }

        public async Task<bool> UpdateInfoAsync(UserInfoReqModel userInfoReqModel)
        {
            var stu = await _dbContext.Students.FirstOrDefaultAsync(x => x.Id == userInfoReqModel.UserId);
            if (stu == null)
            {
                return false;
            }
            stu.Name = userInfoReqModel.Name;
            stu.Mobile = userInfoReqModel.Mobile;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePwdAsync(Guid id, string pwd)
        {
            var stu = await _dbContext.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (stu == null)
            {
                return false;
            }
            stu.Password = pwd;
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
