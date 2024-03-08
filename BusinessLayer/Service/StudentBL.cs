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
    public class StudentBL: IStudentBL
    {
        private readonly IStudentRL studentRL;

        public StudentBL(IStudentRL studentRL)
        {
            this.studentRL = studentRL;
        }

        public Task<string> UserRegistration(Dto _dto)
        {
            return this.studentRL.UserRegistration(_dto);
        }
        public Task<StudentModel> UserLogin(string Email, string Password)
        {
            return this.studentRL.UserLogin(Email,Password);
        }
        /*
        public AuthenticateResponse UserLogin(AuthenticateRequest model)
        {
            return this.studentRL.UserLogin(model);
        }*/

        public IEnumerable<Student> GetAll()
        {
            return this.studentRL.GetAll();
        }
        public Student GetById(int id)
        {
            return this.GetById(id);

        }
    }
}