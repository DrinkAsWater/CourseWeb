using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CourseService.Service
{
    public static class PasswrdHelper
    {
        public static string PwdSHA256Hash(string pwdStr, string salt)
        {
            var result = string.Empty;
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(pwdStr + salt.ToUpper());

                var hash = sha256.ComputeHash(bytes);

                result = Convert.ToBase64String(hash);
            }
            return result;
        }

    }
}
