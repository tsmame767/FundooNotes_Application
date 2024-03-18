using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO;
using RepositoryLayer.Interface;
using BusinessLayer.Interface;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata.Ecma335;
using RepositoryLayer.Service;
using System.IdentityModel.Tokens.Jwt;
using RepositoryLayer.JWT;


namespace FundooNotes.Controllers
{

    [Route("Api/User")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class FundooController : ControllerBase
    {

        ExtractToken ExtractToken=new ExtractToken();
        private readonly IStudentBL service;

        //private readonly StudentRL studentRL;
        
        private readonly IEmailServiceBL emailService;

        //private readonly EmailServiceRL emailServiceRL;

        public FundooController(IStudentBL _service,IEmailServiceBL _emailService) 
        {
            this.service = _service;
            this.emailService = _emailService;
            //this.emailServiceRL= emailServiceRL;
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
        [AllowAnonymous]

        public async Task<IActionResult> GetEmail(string Email)
        {
            
            var res = await this.emailService.SendEmailAsync(Email, "Mail Subject", "Hello Tushar Hope you got the Email");

            
            if (res == 1)
            {
                
                return Ok(new { Message = "Email sent successfully." });
            }
            
            return StatusCode(500, new { Message = "Failed to send email." });
        }

        [HttpPost]
        [Route("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            var user = await this.service.ForgotPassword(model.Email);
            var check = await this.emailService.SendEmailAsync(user.Email, "Password Reset",$"Please Reset your Password {user.CallBack}");
            if (user == null || check ==0)
            {
                return BadRequest();
            }
            
            return Ok(user);
        }

        [HttpPost]
        [Route("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> PasswordReset(PasswordReset model)
        {
            //var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sid);
            var userEmail = ExtractToken.ExtractEmailFromToken(model.Token);
            
            var res = await this.service.ResetPassword(userEmail, model.NewPassword, model.Token);
            if (res != null)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest();
            }


        }








    }
}
