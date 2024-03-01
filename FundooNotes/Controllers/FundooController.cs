using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO;
using RepositoryLayer.Interface;

namespace FundooNotes.Controllers
{
    public class FundooController : ControllerBase
    {
        private readonly IStudentRL service;

        public FundooController(IStudentRL _service) 
        {
            this.service = _service;
        }

        [HttpPost("UserRegistration")]
        public async Task<IActionResult> Create([FromBody] Dto student)
        {
            var _list = await this.service.UserRegistration(student);
            if( _list != null )
            {
                return Ok( _list );
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("UserLogin/{Email}/{Password}")]

        public async Task<IActionResult> UserLog(string Email,string Password)
        {
            var _list = await this.service.UserLogin(Email,Password);
            if (_list != null)
            {
                return Ok(_list);
            }
            else
            {
                return NotFound();
            }
        }

    }
}
