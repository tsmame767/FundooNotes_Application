using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO;
using RepositoryLayer.Interface;
using BusinessLayer.Interface;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata.Ecma335;


namespace FundooNotes.Controllers
{

    [Route("Api/User")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class FundooController : ControllerBase
    {
        private readonly IStudentBL service;

        
        private readonly IEmailServiceBL emailService;

        public FundooController(IStudentBL _service,IEmailServiceBL _emailService) 
        {
            this.service = _service;
            this.emailService = _emailService;
        }

        [HttpPost("UserRegistration")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] UserRegisterRequest Users)
        {
            var _list = await this.service.UserRegistration(Users);
            if( _list != null )
            {
                return Ok( _list );
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("UserLogin")]
        [AllowAnonymous]
        public async Task<IActionResult> UserLog(UserLoginRequest request)
        {
            var _list = await this.service.UserLogin(request);
            if (_list != null)
            {
                return Ok(_list);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("GetAll")]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var list = this.service.GetAll();
            if(list != null )
            {
                return Ok( list );
            }
            else
            {
                return NotFound();
            }
        }

       

        [HttpGet("SendMail")]
        [Authorize]
        public async Task<IActionResult> GetEmail(string Email)
        {
            
            var res = await this.emailService.SendEmailAsync(Email, "Mail Subject", "Hello Tushar Hope you got the Email");

            
            if (res == 1)
            {
                
                return Ok(new { Message = "Email sent successfully." });
            }
            
            return StatusCode(500, new { Message = "Failed to send email." });
        }








    }
}
