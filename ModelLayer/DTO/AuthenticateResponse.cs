using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using RepositoryLayer.Entity;

namespace ModelLayer.DTO
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        /*
        public AuthenticateResponse(Student user,string token)
        {
            Id = user.Id;
            First_Name = user.First_Name;
            Last_Name = user.Last_Name;
            Email = user.Email;
            Token = token;
        }*/
    }
}
