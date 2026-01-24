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
        public async Task<bool> CreateAsync(UserModel user)
        {
            await _dbContext.AddAsync(new Student()
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Password = user.Pwd,
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
                    UserName = student.Name,
                    Email = student.Email,
                    Pwd = student.Password
                };
            }
            return userModel;
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
                    UserName = student.Name,
                    Email = student.Email,
                    Pwd = student.Password
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
