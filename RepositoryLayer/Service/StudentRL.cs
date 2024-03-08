using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.DTO;
using Dapper;
using BCrypt.Net;
using System.Data;
using RepositoryLayer.ContextDB;
using RepositoryLayer.Entity;
using System.Security.Cryptography;
//using RepositoryLayer.HashPassword;
using Azure;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using RepositoryLayer.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace RepositoryLayer.Service
{
    public class StudentRL: IStudentRL
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<Student> _users = new List<Student>
        {
        new Student { Id = 1, First_Name = "Test", Last_Name = "User", Email = "test", Password = "test" }
        };


        private readonly ContextDataBase _context;
        private readonly AppSettings _appSettings;
        public StudentRL(ContextDataBase context,IOptions<AppSettings> appSettings)
        {
            this._context = context;
            this._appSettings=appSettings.Value;

        }





        public async Task<string> UserRegistration(Dto _dto)
        {
            string response = string.Empty;
            

            var query = "insert into Student_Details (first_name,last_name,email,[password]) values (@first_name,@last_name,@email,@password)";

            var parameters = new DynamicParameters();
            parameters.Add("first_name",_dto.First_Name,DbType.String);
            parameters.Add("last_name", _dto.Last_Name, DbType.String);
            parameters.Add("email",_dto.Email, DbType.String);
            parameters.Add("password", _dto.Password, DbType.String);

            // Hash the password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(_dto.Password);

            parameters.Add("Password", hashedPassword, DbType.String); // Use the hashed password


            using (var connect = this._context.CreateConnection())
            {
                await connect.ExecuteAsync(query, parameters);
                response = "Registration successful";


            }

            return response;
        }


        public async Task<StudentModel> UserLogin(string email, string password)
        {

            string response = "Login Failed : No Details Matched";
            var query = "select * from Student_Details where email=@email";
            StudentModel student = null;
            using (var connect = this._context.CreateConnection())
            {
                var user = await connect.QueryFirstOrDefaultAsync<Student>(query, new { Email = email });
                if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    // authentication successful so generate jwt token
                    var token = generateJwtToken(user);
                    response = "Login Successful";
                    student = new StudentModel()
                    {
                        Id = user.Id,
                        Email=user.Email,
                        First_Name=user.First_Name,
                        Last_Name=user.Last_Name,
                        token= token
                    };

                }
            }
            

            return student;

            //return response;
        }

        /*
        public AuthenticateResponse UserLogin(AuthenticateRequest model)
        {
            var user = _users.SingleOrDefault(x => x.Email == model.Email && x.Password == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }
        public StudentModel UserLogin(string Email, string Password)
        {
            var user = _users.SingleOrDefault(x => x.Email == Email && x.Password == Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            StudentModel student = new StudentModel();
            student.Email = user.Email;
            student.First_Name = user.First_Name;   
            student.Last_Name = user.Last_Name;
            student.token = token;
            student.Id = user.Id;

            return student;
        }*/
        
        public IEnumerable<Student> GetAll()
        {
            return _users;
        }

        public Student GetById(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }

        //helper methods

        private string generateJwtToken(Student user)
        {
            //generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
