using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO;
using RepositoryLayer.Interface;
using BusinessLayer.Interface;





namespace FundooNotes.Controllers
{

    [Route("Api/User")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class FundooController : ControllerBase
    {
        private readonly IStudentBL service;

        public FundooController(IStudentBL _service) 
        {
            this.service = _service;
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

         

    }
}
