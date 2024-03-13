using Dapper;
using ModelLayer.DTO;
using RepositoryLayer.ContextDB;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class CollabRL:ICollabRL
    {
        private readonly ContextDataBase _context;
        public CollabRL(ContextDataBase contextData) 
        { 
            this._context = contextData;
        }
        public async Task<List<Collabinfo>> GetAll()
        {
            var query = "select * from collaboration";
            using (var connect = this._context.CreateConnection())
            {
                var res = await connect.QueryAsync<Collabinfo>(query);
                return res.ToList();
            }
        }

        public int AddCollaborator(int NoteId, CollabRequest Request, int UserId)
        {
            var res = 0;
            var query = "insert into Collaboration (userId,NoteId,collaboratorsEmail) values(@userId,@NoteId,@collaboratorsEmail)";
            using (var connect = this._context.CreateConnection())
            {
                res = connect.Execute(query, new { UserId = UserId, NoteId = NoteId, collaboratorsEmail = Request.Email });
            }
            return res;
        }

        public int RemoveCollaborator(int NoteId, CollabRequest Request)
        {
            var res = 0;
            var query = "delete from Collaboration where NoteId=@NoteId and collaboratorsEmail=@Email";
            using (var connect = this._context.CreateConnection())
            {
                res = connect.Execute(query, new { NoteId = NoteId, Email = Request.Email });
            }
            return res;
        }
    }
}
