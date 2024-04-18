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
using Microsoft.AspNetCore.Cors;


namespace FundooNotes.Controllers
{

    [Route("Api/User")]
    [ApiController]
    [EnableCors]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {

        ExtractToken ExtractToken=new ExtractToken();
        private readonly IUserBL service;

        private readonly ILogger<UserController> logger;
        private readonly IEmailServiceBL emailService;
        public UserController(IUserBL _service, IEmailServiceBL _emailService, ILogger<UserController> logger)
        {
            this.service = _service;
            this.emailService = _emailService;
            this.logger = logger;
            logger.LogDebug("Nlog is integrated to Book Controller");
            //this.emailServiceRL= emailServiceRL;
        }

        [HttpPost("UserRegistration")]
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid user registration request.");
            }

            try
            {
                var response = await service.UserRegistration(request);

                if (response.IsSuccess)
                {
                    // If registration is successful
                    return Ok(response);
                }
                else
                {
                    // If registration is not successful, but no exception was thrown
                    return BadRequest(response);
                }
            }
            catch (DuplicateEmailException ex)
            {
                logger.LogError(ex.Message);
                // Handle specific known exception types differently if needed
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Generic exception handler for unexpected errors
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
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
