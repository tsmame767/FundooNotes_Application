﻿using System;
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

        

        IEnumerable<Student> GetAll();
        Student GetById(int id);
        public Task<string> UserRegistration(Dto _dto);
        public Task<StudentModel> UserLogin(string Email, string Password);
        //public AuthenticateResponse UserLogin(AuthenticateRequest model);
    }

    
}

    

