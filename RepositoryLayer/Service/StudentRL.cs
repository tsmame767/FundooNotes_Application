using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.DTO;
using Dapper;
using System.Data;
using RepositoryLayer.ContextDB;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Service
{
    public class StudentRL: IStudentRL
    {
        private readonly ContextDataBase _context;
        public StudentRL(ContextDataBase context)
        {
            this._context = context;
        }
        public async Task<string> UserRegistration(Dto _dto)
        {
            string response = string.Empty;
            var query = "insert into Student_Details(first_name,last_name,email,[password]) values(@First_Name,@Last_Name,@Email,@password)";

            var parametres = new DynamicParameters();
            parametres.Add("first_name", _dto.First_Name, DbType.String);
            parametres.Add("last_name", _dto.Last_Name, DbType.String);
            parametres.Add("email", _dto.Email, DbType.String);
            parametres.Add("password", _dto.Password, DbType.String);

            using (var connect = this._context.CreateConnection())
            {
                await connect.ExecuteAsync(query, parametres);
                response = "pass";

                var inserted = new Student
                {
                    First_Name = _dto.First_Name,
                    Last_Name = _dto.Last_Name,
                    Email = _dto.Email,
                    Password = _dto.Password,

                };
            }
            return response;
        }

        public async Task<string> UserLogin(string email, string password)
        {
            string response = "No Details Matched";
            var query = "Select * from Student_Details where email=@email and [password]=@Password";

            using(var connect = this._context.CreateConnection())
            {
                var emplist=await connect.QueryFirstOrDefaultAsync<Student>(query, new { email, password });
                response = "Login SuccessFul";
            }
            return response;
        }
    }
}
