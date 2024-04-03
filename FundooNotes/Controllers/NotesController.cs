using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> Get()
        {
            // Attempt to find the UserId claim
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sid);
            int userId = int.Parse(userIdClaim.Value);

            // Redis Cache Check
            var cacheKey = $"User_{userId}";
            var cachedData = CacheService.GetData<Note>(cacheKey);
            if (cachedData != null)
            {
                return Ok(cachedData);
            }

            var list =  await NoteService.GetAll(userId);
            if (list != null)
            {
                CacheService.SetData(cacheKey, list,DateTimeOffset.Now.AddMinutes(5));
                return Ok(list);
            }
            return NotFound();
        }
        
        [HttpPost]
        [Authorize]
        public  NoteResponse Create([FromBody] CreateNoteRequest Request)
        {
            
            NoteResponse response = new NoteResponse();
            // Attempt to find the UserId claim
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sid);
            int userId = int.Parse(userIdClaim.Value);
            
            var note =  this.NoteService.CreateNote(Request, userId);
            var cacheKey = $"Note_{note.NoteId}";

            if (note!=null )
            {

                // Retrieve existing notes from cache or initialize a new list
                //var notesList = CacheService.GetData<List<NoteInfo>>(cacheKey) ?? new List<NoteInfo>();
                var notes= CacheService.GetData<List<Note>>(cacheKey) ?? new List<Note>();


                notes.Add(note);
                
                // Update the cache with the new list of notes
                CacheService.SetData(cacheKey, notes, DateTimeOffset.Now.AddMinutes(5));

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
        public NoteResponse Update(int NoteId, UpdateNoteRequest Request)
        {
            NoteResponse response=new NoteResponse();
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sid);
            int userId = int.Parse(userIdClaim.Value);
            
            var res = this.NoteService.UpdateNote(Request,userId,NoteId);
            if (Convert.ToInt32(res) > 0)
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
        public NoteResponse Delete(int NoteId)
        {
            NoteResponse response=new NoteResponse();
            var res = this.NoteService.DeleteNote(NoteId);
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
        public NoteResponse isArchived(int NoteId)
        {
            NoteResponse response=new NoteResponse();
            response.Status = 204;
            response.Message = "Note Not Archived";
            response.IsSuccess = false;

            var res = this.NoteService.isArchived(NoteId);
            if (res == true) 
            {
                response.Status = 200;
                response.Message = "Note Archived";
                response.IsSuccess = true;
            }

            return response;
        }

        [HttpPatch("Trash/{NoteId}")]
        [Authorize]
        public NoteResponse isDeleted(int NoteId)
        {
            NoteResponse response = new NoteResponse();
            response.Status = 204;
            response.Message = "Note Not Trashed";
            response.IsSuccess = false;
            var res = this.NoteService.isDeleted(NoteId);
            if (res == true)
            {
                response.Status = 200;
                response.Message = "Note Trashed";
                response.IsSuccess = true;
            }
            return response;
        }


    }



        
    
}
