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
using System.Collections;

namespace RepositoryLayer.Service
{
    public class NoteRL:INoteRL
    {
        private readonly ContextDataBase context;
        private readonly ICacheServiceRL CacheService;

        public NoteRL(ContextDataBase context, ICacheServiceRL cacheService)
        {
            this.context = context;
            CacheService = cacheService;
        }

        public async Task<List<Note>> GetAll(int userId)
        {
            var query = $"select * from Notes where userid={userId}";
            using (var connect = this.context.CreateConnection())
            {
                var list = await connect.QueryAsync<Note>(query);
                return list.ToList();
            }
        }
        
        public Note CreateNote(CreateNoteRequest Request, int UserId)
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
                var note = connect.QueryFirstOrDefault<Note>("SELECT TOP 1 * FROM notes ORDER BY noteid DESC");

                return new Note { Colour = Request.Colour, Title = Request.Title, Description = Request.Description,UserId=UserId, IsArchived=false,IsDeleted=false,NoteId=note.NoteId};
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
        public async Task<int> UpdateNote(UpdateNoteRequest Request, int UserId,int NoteId)
        {
            var cacheKey = $"User_{UserId}";

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

                //updating note

                //var notes=CacheService.GetData(cacheKey);
                CacheService.RemoveData(cacheKey);
                var list = await connect.QueryAsync<Note>($"select * from Notes where userid={UserId}");
                CacheService.SetData($"User_{UserId}", list.ToList(), DateTimeOffset.Now.AddMinutes(5));

                return res2;
            }

        }
        public async Task<int> DeleteNote(int NoteId,int UserId)
        {
            string CacheKey = $"User_{UserId}";
            var res = -1;
            var query = "Delete from Notes where noteid=@NoteId";

            using (var connect = this.context.CreateConnection())
            {
                res=connect.Execute(query, new { noteid = NoteId });
                if (res <= 0)
                {
                    return res;
                }
                CacheService.RemoveData($"User_{UserId}");
                var retnotes = await connect.QueryAsync<Note>($"select * from Notes where userid={UserId}");
                CacheService.SetData(CacheKey, retnotes, DateTimeOffset.Now.AddMinutes(5));

            }
            //CacheService.RemoveData($"Note_{NoteId}");
            return res;
        }

        public async Task<string> isArchived(int NoteId,int UserId)
        {
            string CacheKey = $"User_{UserId}";
            var res = -1;

            
            var query = "update Notes set isarchived=1 where noteid=@noteid and UserId=@UserId";
            using (var connect = this.context.CreateConnection())
            {
                //check note archive
                var ArchiveNoteCheck = await connect.QueryFirstOrDefaultAsync("select isArchived from Notes where noteid=@NoteId and UserId=@UserId", new { NoteId = NoteId, UserId = UserId });
                if(ArchiveNoteCheck?.isArchived)
                {
                    query= "update Notes set isarchived=0 where noteid=@noteid and UserId=@UserId";
                }

                res =connect.Execute(query, new { noteid = NoteId, UserId=UserId });
                if (res >= 0)
                {
                    CacheService.RemoveData($"User_{UserId}");
                    var retnotes = await connect.QueryAsync<Note>($"select * from Notes where userid={UserId}");
                    CacheService.SetData(CacheKey, retnotes, DateTimeOffset.Now.AddMinutes(5));

                    if (ArchiveNoteCheck?.isArchived)
                    {
                        return "UnArchived";
                    }
                    return "Archived";
                }
                
            }
            return null;
        }

        public async Task<string> isDeleted(int NoteId,int UserId)
        {
            string CacheKey = $"User_{UserId}";
            var res = -1;
            var query = "update Notes set isdeleted=1 where noteid=@noteid and UserId=@UserId";
            using (var connect = this.context.CreateConnection())
            {
                //check note trash
                var TrashNoteCheck = await connect.QueryFirstOrDefaultAsync("select isDeleted from Notes where noteid=@NoteId and UserId=@UserId", new { NoteId = NoteId, UserId = UserId });
                if (TrashNoteCheck?.isDeleted)
                {
                    query = "update Notes set isdeleted=0 where noteid=@noteid and UserId=@UserId";
                }


                res = connect.Execute(query, new { noteid = NoteId , UserId=UserId });
                if (res >= 0)
                {
                    CacheService.RemoveData($"User_{UserId}");
                    var retnotes = await connect.QueryAsync<Note>($"select * from Notes where userid={UserId}");
                    CacheService.SetData(CacheKey, retnotes, DateTimeOffset.Now.AddMinutes(5));

                    if (TrashNoteCheck?.isDeleted)
                    {
                        return "UnTrashed";
                    }
                    return "Trashed";

                }
                
            }
            return null;
        }
    }
}
