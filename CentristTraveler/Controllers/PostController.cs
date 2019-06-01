using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CentristTraveler.BusinessLogic.Interfaces;
using CentristTraveler.Dto;
using CentristTraveler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace CentristTraveler.Controllers
{

    [Route("api/[controller]")]
    public class PostController : Controller
    {
        private IPostBL _postBL;
        private IHostingEnvironment _hostingEnvironment;
        public PostController(IPostBL postBL, IHostingEnvironment hostingEnvironment)
        {
            _postBL = postBL;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [Route("SearchPosts")]
        public IEnumerable<PostDto> SearchPosts([FromBody] PostSearchParamDto searchParam)
        {
            List<PostDto> postDtos = _postBL.SearchPosts(searchParam);
            return postDtos;
        }

        [HttpGet]
        [Route("GetLatestPosts")]
        public IEnumerable<PostDto> GetLatestPosts()
        {
            List<PostDto> postDtos = _postBL.GetLatestPosts();
            return postDtos;
        }

        [HttpPost]
        [Route("AddPost")]
        [Authorize(Roles = "Writer")]
        public IActionResult AddPost([FromBody] PostDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            return Ok(_postBL.Create(postDto));
        }
        [HttpGet]
        [Route("Update/{id}")]
        public IActionResult Update(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            PostDto postDto = _postBL.GetPostById(id);
            
            return Ok(postDto);
        }

        [HttpPost]
        [Route("Update")]
        public IActionResult Update([FromBody] PostDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            return Ok(_postBL.Update(postDto));
        }
        [HttpGet]
        [Route("Detail/{id}")]
        public IActionResult Detail(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            PostDto postDto = _postBL.GetPostById(id);
            return Ok(postDto);
        }

        [HttpGet]
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(_postBL.Delete(id));
        }

        [HttpGet]
        [Route("GetAllCategories")]
        public IActionResult GetAllCategories()
        {
            return Ok(_postBL.GetAllCategories());
        }

        [HttpGet]
        [Route("GetPopularTags")]
        public IActionResult GetPopularTags()
        {
            return Ok(_postBL.GetPopularTags());
        }
    }
}
