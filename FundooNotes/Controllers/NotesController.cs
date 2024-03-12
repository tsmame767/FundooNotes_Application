using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO;
using RepositoryLayer.Entity;
using System.IdentityModel.Tokens.Jwt;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class NotesController : ControllerBase
    {
        private readonly INoteBL NoteService;

        public NotesController(INoteBL _Service)
        {
            this.NoteService = _Service;
        }


        [HttpGet]
        [Authorize] 
        public async Task<IActionResult> Get()
        {
            var list =  await NoteService.GetAll();
            if (list != null)
            {
                return Ok(list);
            }
            else
            {
                return BadRequest();
            }
        }
        
        [HttpPost]
        [Authorize]
        public  NoteResponse Create([FromBody] CreateNoteRequest Request)
        {
            NoteResponse response = new NoteResponse();
            // Attempt to find the UserId claim
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sid);
            int userId = int.Parse(userIdClaim.Value);
            var _list =  this.NoteService.CreateNote(Request, userId);
            if (Convert.ToInt32(_list)> 0 )
            {
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
