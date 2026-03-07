using CourseApi.Response;
using CourseService.Models;

namespace CourseApi.security
{
    public interface IJwtHelper
    {
        LoginResponse GenerateToken(UserModel user);
    }
}
