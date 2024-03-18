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

    public class User
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; } // Expiration time for the reset token
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
        public string CallBack { get; set; }
        //public string Token { get; set; }

    }

    public class PasswordReset
    {
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
