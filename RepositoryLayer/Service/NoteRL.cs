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

        public string CheckInp(string newstr,string oldstr)
        {
            if (newstr == "")
            {
                return oldstr;
            }
            return newstr;
        }
        //public UpdateNoteRequest UpdateNote(UpdateNoteRequest Request, int UserId)
        public int UpdateNote(UpdateNoteRequest Request, int UserId,int NoteId)
        {
            var query1 = "select noteid, description,title,colour from Notes where userid=@userid and noteid=@noteid";

            var query2 = "update Notes set description=@description, title=@title, colour=@colour where userid=@userId and noteid=@noteid";
            String PrevTitle, PrevDescription, PrevColour;

            //Data Retrieval Suite
            var parameters1 = new DynamicParameters();
            
            parameters1.Add("title", Request.Title, DbType.String);
            parameters1.Add("description", Request.Description, DbType.String);
            parameters1.Add("colour", Request.Colour, DbType.String);
            using (var connect = this.context.CreateConnection())
            {
                //Data Update Suite
                var res1 = connect.Query<UpdateNoteRequest>(query1, new { UserId = UserId, noteid = NoteId });
                var RetResults = res1.SingleOrDefault();
                if (RetResults == null)
                {
                    return -1;
                }
                PrevTitle = RetResults.Title;
                PrevColour = RetResults.Colour;
                PrevDescription = RetResults.Description;

                var res2 = connect.Execute(query2, new { title = CheckInp(Request.Title, PrevTitle), description = CheckInp(Request.Description,PrevDescription), colour = CheckInp(Request.Colour,PrevColour), noteid = NoteId, userId = UserId, });
                return res2;
            }

        }
        public int DeleteNote(int NoteId)
        {
            var res = -1;
            var query = "Delete from Notes where noteid=@NoteId";

            using (var connect = this.context.CreateConnection())
            {
                res=connect.Execute(query, new { noteid = NoteId });
                if (res <= 0)
                {
                    return res;
                }
            }
            return res;
        }

        public bool isArchived(int NoteId)
        {
            return false;
        }
    }
}
