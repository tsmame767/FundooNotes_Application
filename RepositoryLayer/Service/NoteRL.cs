using Dapper;
using RepositoryLayer.ContextDB;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Net.WebSockets;
using ModelLayer.DTO;

namespace RepositoryLayer.Service
{
    public class NoteRL:INoteRL
    {
        private readonly ContextDataBase context;

        public NoteRL(ContextDataBase context)
        {
            this.context = context;
        }

        public async Task<List<Note>> GetAll()
        {
            var query = "select * from Notes";
            using (var connect = this.context.CreateConnection())
            {
                var list = await connect.QueryAsync<Note>(query);
                return list.ToList();
            }
        }
        
        public int CreateNote(CreateNoteRequest Request, int UserId)
        {
            
            var query = "insert into notes(description,title,colour,userid,isarchived,isdeleted) values(@description,@title,@colour,@userid,@isarchived,@isdeleted)";
            
            var parameters = new DynamicParameters();
            parameters.Add("description", Request.Description, DbType.String);
            parameters.Add("title", Request.Title, DbType.String);
            parameters.Add("colour", Request.Colour, DbType.String);
            parameters.Add("isarchived", false, DbType.Boolean);
            parameters.Add("isdeleted",false, DbType.Boolean);
            parameters.Add("userid",UserId, DbType.Int32);

            using (var connect = this.context.CreateConnection())
            {
                var execute = connect.Execute(query,parameters);

                return execute;
            }
        }
    }
}
