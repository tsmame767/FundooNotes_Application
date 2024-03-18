using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
    public interface IStudentRL
    {

        

        //IEnumerable<Users> GetAll();
        //Users GetById(int id);
        public Task<UserRegisterResponse> UserRegistration(UserRegisterRequest Users);
        public Task<UserLoginResponse> UserLogin(UserLoginRequest request);

        public Task<List<Users>> GetAll();
        public Task<PasswordResetModel> ForgotPassword(string email);
        public Task<PasswordResponse> ResetPassword(string email, string NewPass, string Token);
        //public AuthenticateResponse UserLogin(AuthenticateRequest model);
    }

    
}

    

