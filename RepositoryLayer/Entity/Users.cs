using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace RepositoryLayer.Entity
{
    public class Users
    {

        public int Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public string Password { get; set; } //salt password
        //public string HashPassword { get; set; } //hash password




    }
}
