using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Entity;

namespace RepositoryLayer.ContextDB
{
    public class ContextDataBase
    {
        private readonly IConfiguration config;
        private readonly string connectStr;

        public ContextDataBase(IConfiguration config)
        {
            this.config = config;
            this.connectStr = this.config.GetConnectionString("SqlConnection");
        }

        public IDbConnection CreateConnection() => new SqlConnection(this.connectStr);


    }
}
