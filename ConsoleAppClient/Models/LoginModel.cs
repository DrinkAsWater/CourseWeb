using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppClient.Models
{
   public class LoginResponse
    {
       public string Token { get; set; } = string.Empty;

       public string Username { get; set; } = string.Empty;


    }

    public class LoginRequest
    {
        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
