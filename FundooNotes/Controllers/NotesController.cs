using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.DTO;
using RepositoryLayer.Entity;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class NotesController : ControllerBase
    {
        private readonly INoteBL NoteService;
        private readonly ICacheService CacheService;

        public NotesController(INoteBL _Service, ICacheService cacheService)
        {
            this.NoteService = _Service;
            CacheService = cacheService;
        }


        [HttpGet]
        [Authorize] 
        public async Task<IActionResult> GetNotes()
        {
            // Attempt to find the UserId claim
            var UserIdClaim = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sid);
            int UserId = int.Parse(UserIdClaim.Value);
            
            // Redis Cache Check
            var CacheKey = $"User_{UserId}";
            var CachedData = CacheService.GetData(CacheKey);
            if (!CachedData.IsNullOrEmpty())
            {
                return Ok(CachedData);
            }

            var list =  await NoteService.GetAll(UserId);
            if (!list.IsNullOrEmpty())
            {
                CacheService.SetData(CacheKey, list,DateTimeOffset.Now.AddMinutes(5));
                return Ok(list);
            }
            return NotFound();
        }
        
        [HttpPost]
        [Authorize]
        public  NoteResponse CreateNotes( CreateNoteRequest Request)
        {
            
            NoteResponse response = new NoteResponse();
            // Attempt to find the UserId claim
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sid);
            int User_Id = int.Parse(userIdClaim.Value);
            
            var note =  this.NoteService.CreateNote(Request, User_Id);
            var cacheKey = $"User_{User_Id}";

            if (note!=null )
            {
                
                    // Retrieve existing notes from cache or initialize a new list
                    //var notesList = CacheService.GetData<List<NoteInfo>>(cacheKey) ?? new List<NoteInfo>();
                    //var notes= CacheService.GetData(cacheKey) ?? new List<Note>();


                    //notes.Add(note);

                    // Update the cache with the new list of notes
                    var notes = CacheService.GetData(cacheKey);
                    if (notes != null)
                    {
                        notes.Add(note);
                        CacheService.SetData(cacheKey, notes, DateTimeOffset.Now.AddMinutes(5));
                    }

                    response.Status = 200;
                    response.Message = "Note Added Successfully";
                    response.IsSuccess = true;
                               
            }

            
            else
            {
                response.Status = 404;
                response.Message = "Note Not Added";
                response.IsSuccess = false;
               
            }
            return response;
        }
        [HttpPut]
        [Authorize]
        public NoteResponse UpdateNotes(int NoteId, UpdateNoteRequest Request)
        {
            NoteResponse response=new NoteResponse();
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sid);
            int userId = int.Parse(userIdClaim.Value);
            
            var res = this.NoteService.UpdateNote(Request,userId,NoteId);
            if (res!=null)
            {
                response.Status = 201;
                response.Message = "Note Updated Successfully";
                response.IsSuccess = true;
            }

            else
            {
                response.Status = 404;
                response.Message = "Note Not Update";
                response.IsSuccess = false;

            }
            return response;
        }

        [HttpDelete]
        [Authorize]
        public async Task<NoteResponse> DeleteNotes(int NoteId)
        {
            NoteResponse response=new NoteResponse();
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sid);
            int UserId = int.Parse(userIdClaim.Value);
            var res = await this.NoteService.DeleteNote(NoteId,UserId);
            if (Convert.ToInt32(res) > 0)
            {
                response.Status = 204;
                response.Message = "Note Deleted Successfully";
                response.IsSuccess = true;

            }

            else
            {
                response.Status = 404;
                response.Message = "Note Not Deleted";
                response.IsSuccess = false;

            }
            return response;

        }

        [HttpPatch("Archive/{NoteId}")]
        [Authorize]
        public async Task<NoteResponse> IsArchived(int NoteId)
        {
            NoteResponse response=new NoteResponse();
            response.Status = 204;
            response.Message = "Note Not Archived";
            response.IsSuccess = false;

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sid);
            int UserId = int.Parse(userIdClaim.Value);

            var res = await this.NoteService.isArchived(NoteId, UserId);
            if (res=="Archived") 
            {
                response.Status = 200;
                response.Message = "Note Archived";
                response.IsSuccess = true;
            }
            else if (res == "UnArchived")
            {
                response.Status = 200;
                response.Message = "Note UnArchived";
                response.IsSuccess = true;
            }

            return response;
        }

        [HttpPatch("Trash/{NoteId}")]
        [Authorize]
        public async Task<NoteResponse> IsDeleted(int NoteId)
        {
            NoteResponse response = new NoteResponse();
            response.Status = 204;
            response.Message = "Note Not Trashed";
            response.IsSuccess = false;

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sid);
            int UserId = int.Parse(userIdClaim.Value);

            var res = await this.NoteService.isDeleted(NoteId, UserId);
            if (res=="Trashed")
            {
                response.Status = 200;
                response.Message = "Note Trashed";
                response.IsSuccess = true;
            }
            else
            {
                response.Status = 200;
                response.Message = "Note UnTrashed";
                response.IsSuccess = true;
            }
            return response;
        }


    }



        
    
}
