using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL UserRL;

        public UserBL(IUserRL UserRL)
        {
            this.UserRL = UserRL;
        }

        public Task<UserRegisterResponse> UserRegistration(UserRegisterRequest Users)
        {
            return this.UserRL.UserRegistration(Users);
        }

        
        public Task<UserLoginResponse> UserLogin(UserLoginRequest request)
        {
            return this.UserRL.UserLogin(request);
        }

        public Task<List<Users>> GetAll()
        {
            return this.UserRL.GetAll();
        }

        public Task<PasswordResetModel> ForgotPassword(string email)
        {
            return this.UserRL.ForgotPassword(email);
        }

        public Task<PasswordResponse> ResetPassword( string email, string NewPass, string Token)
        {
            return this.UserRL.ResetPassword(email,NewPass, Token);
        }


    }
}