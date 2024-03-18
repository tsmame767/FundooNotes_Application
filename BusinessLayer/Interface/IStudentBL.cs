using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ModelLayer.DTO;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interface
{
    public interface IStudentBL


    {
        public Task<UserRegisterResponse> UserRegistration(UserRegisterRequest Users);

        public Task<UserLoginResponse> UserLogin(UserLoginRequest request);

        public Task<List<Users>> GetAll();

        public Task<PasswordResetModel> ForgotPassword(string email);
        public Task<PasswordResponse> ResetPassword(string email,string NewPass,string Token);
        //public AuthenticateResponse UserLogin(AuthenticateRequest model);
    }
}
