using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentristTraveler.BusinessLogic.Interfaces;
using CentristTraveler.Dto;
using CentristTraveler.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CentristTraveler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticationBL _authenticationBL;
        public AuthenticationController(IAuthenticationBL authenticationBL)
        {
            _authenticationBL = authenticationBL;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isSuccess = await _authenticationBL.Register(userDto);
            
            return Ok();
            
        }

        [HttpPost]
        [Route("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string token = await _authenticationBL.Authenticate(userDto.Username, userDto.Password);

            return Ok(token);
        }
    }
}