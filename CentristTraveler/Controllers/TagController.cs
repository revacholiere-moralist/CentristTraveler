using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentristTraveler.Dto;
using CentristTraveler.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CentristTraveler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        [HttpPost]
        [Route("AddTag")]
        public IActionResult AddTag([FromBody] TagDto tagDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Tag tag = new Tag
            {
                Name = tagDto.Name
            };

            return Ok();
        }
    }
}