using BusinessLayer.Interface;
using ModelLayer.DTO;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Task<string> UserLogin(string email, string password)
        {
            return this.studentRL.UserLogin(email, password);
        }
    }
}
