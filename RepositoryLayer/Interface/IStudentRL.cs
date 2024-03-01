using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.DTO;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
    public interface IStudentRL
    {
        public Task<string> UserRegistration(Dto _dto);

        public Task<string> UserLogin(string email, string password);
    }

    
}

    

