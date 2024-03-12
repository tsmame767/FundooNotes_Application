using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTO
{
    public class UserLoginRequest
    {
        [Required(ErrorMessage ="Email is Mandatory")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Mandatory")]
        public string Password { get; set; }
    }

    public class UserLoginResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public UserLoginInformation data { get; set; }
        public string Token { get; set; }

    }

    public class UserLoginInformation
    {
        public int Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }

    }

}
