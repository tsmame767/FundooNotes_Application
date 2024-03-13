using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO;
using System.IdentityModel.Tokens.Jwt;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteCollabController : ControllerBase
    {
        private readonly ICollabBL CollabService;

        public NoteCollabController(ICollabBL _Service)
        {
            this.CollabService = _Service;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var list = await CollabService.GetAll();
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
        public CollabResponse AddCollab(int NoteId,CollabRequest Request)
        {
            CollabResponse response = new CollabResponse();
            response.Status = 204;
            response.Message = "Could Add Collaborator";
            response.IsSuccess = false;

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sid);
            int userId = int.Parse(userIdClaim.Value);
            var res = this.CollabService.AddCollaborator(NoteId,Request,userId);
            if (res > 0)
            {
                response.IsSuccess = true;
                response.Message = "Collaborator Added";
                response.Status = 201;
            }

            return response;
        }

        [HttpDelete]
        public CollabResponse RemoveCollab(int NoteId, CollabRequest Request)
        {
            CollabResponse response = new CollabResponse();
            response.Status = 404;
            response.Message = "Could Not Delete Collaborator";
            response.IsSuccess = false;

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sid);
            int userId = int.Parse(userIdClaim.Value);
            var res = this.CollabService.RemoveCollaborator(NoteId, Request);
            if (res > 0)
            {
                response.IsSuccess = true;
                response.Message = "Collaborator Deleted";
                response.Status = 204;
            }

            return response;
        }



    }
}
