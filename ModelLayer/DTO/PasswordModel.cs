using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTO
{
    public class ForgotPasswordModel
    { 
        public string Email { get; set; }

    }

    public class PasswordResponse
    {
        public int Status {  get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }

    }

    public class PasswordResetModel
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string Token { get; set; }

    }

    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public string FromEmail { get; set; }
    }
}
