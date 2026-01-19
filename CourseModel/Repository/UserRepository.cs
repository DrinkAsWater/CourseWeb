using CourseService.Interface;
using CourseService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseData.Repository
{
 
    public class UserRepository : IUserRespository
    {
        private readonly IUserRespository _userRespository;

        public UserRepository(IUserRespository userRespository)
        {
            _userRespository = userRespository;
        }
        public Task<bool> CreateAsync(UserModel user)
        {
            throw new NotImplementedException();
        }
    }
}
