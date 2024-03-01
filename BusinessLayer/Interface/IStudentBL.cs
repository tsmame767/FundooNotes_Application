using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ModelLayer.DTO;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interface
{
    public interface IStudentBL
    {
        public Task<string> UserRegistration(Dto _dto);

        public Task<string> UserLogin(string email, string password);
    }
}
