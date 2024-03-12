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
    public class StudentBL : IStudentBL
    {
        private readonly IStudentRL studentRL;

        public StudentBL(IStudentRL studentRL)
        {
            this.studentRL = studentRL;
        }

        public Task<UserRegisterResponse> UserRegistration(UserRegisterRequest Users)
        {
            return this.studentRL.UserRegistration(Users);
        }

        
        public Task<UserLoginResponse> UserLogin(UserLoginRequest request)
        {
            return this.studentRL.UserLogin(request);
        }

        public Task<List<Users>> GetAll()
        {
            return this.studentRL.GetAll();
        }


    }
}