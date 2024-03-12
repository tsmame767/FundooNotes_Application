﻿using RepositoryLayer.Interface;
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

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using RepositoryLayer.JWT;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace RepositoryLayer.Service
{
    public class StudentRL: IStudentRL
    {
        

        private readonly ContextDataBase _context;
        private readonly IConfiguration _config;
    
        public StudentRL(ContextDataBase context,IConfiguration config)
        {
            this._context = context;
            this._config = config;
            


        }





        public async Task<UserRegisterResponse> UserRegistration(UserRegisterRequest Users)
        {
            UserRegisterResponse response = new UserRegisterResponse();
            response.IsSuccess = false;
            response.Message = "no data registered";
            try
            {

                var query = "insert into Student_Details (first_name,last_name,email,[password]) values (@first_name,@last_name,@email,@password)";

                var parameters = new DynamicParameters();
                parameters.Add("first_name", Users.First_Name, DbType.String);
                parameters.Add("last_name", Users.Last_Name, DbType.String);
                parameters.Add("email", Users.Email, DbType.String);
                parameters.Add("password", Users.Password, DbType.String);

                // Hash the password
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(Users.Password);

                parameters.Add("Password", hashedPassword, DbType.String); // Use the hashed password


                using (var connect = this._context.CreateConnection())
                {
                    await connect.ExecuteAsync(query, parameters);
                    response.IsSuccess = true;
                    response.Message = "Registeration Succesful";
                    response.Status = 200;


                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Status = 204;
            }

            return response;
        }


        public async Task<UserLoginResponse> UserLogin(UserLoginRequest request)
        {
            UserLoginResponse response = new UserLoginResponse();
            UserLoginResponse UserResp = null;
            response.IsSuccess = true;
            response.Message = "Successful";
            try
            {
                //string response = "Login Failed : No Details Matched";
                var query = "select * from Student_Details where email=@Email";
                
                using (var connect = this._context.CreateConnection())
                {

                    var user = await connect.QueryFirstOrDefaultAsync<Users>(query, new { Email = request.Email });
                    if (user != null && BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                    {

                        


                        TokenGenerator token = new TokenGenerator(_config);
                        response.Message = "Login Successful";
                        response.data = new UserLoginInformation();
                        response.data.Id = user.Id;
                        response.data.Email = user.Email;
                        response.data.First_Name = user.First_Name;
                        response.data.Last_Name = user.Last_Name;
                        response.Token = token.generateJwtToken(response.data.Id, response.data.Email);
                        // authentication successful so generate jwt token
                       
                        //response = "Login Successful";
                        //UserResp = new UserLoginResponse()
                        //{

                        //    //id = user.id,
                        //    //email=user.email,
                        //    //first_name=user.first_name,
                        //    //last_name=user.last_name,
                        //    //token= token.generatejwttoken(user)
                        //};

                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Login Unsuccesful";
                        return response;
                    }
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess=false;
                response.Message = ex.Message;
            }
            finally
            {
                //
            }
            //return UserResp;

            return response;
        }

        public async Task<List<Users>> GetAll()
        {

            var query = "select*from Student_Details";
            try
            {
                using (var connect = this._context.CreateConnection())
                {
                    var execute = await connect.QueryAsync<Users>(query);
                    return execute.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
