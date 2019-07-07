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
        public async Task<IEnumerable<PostDto>> SearchPosts([FromBody] PostSearchParamDto searchParam)
        {
            return await _postBL.SearchPosts(searchParam);
        }

        [HttpGet]
        [Route("GetLatestPosts")]
        public async Task<IEnumerable<PostDto>> GetLatestPosts()
        {
            return await _postBL.GetLatestPosts();
        }

        [HttpGet]
        [Route("GetPopularPosts")]
        public async Task<IEnumerable<PostDto>> GetPopularPosts()
        {
            return await _postBL.GetPopularPosts();         
        }
        [HttpPost]
        [Route("AddPost")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> AddPost([FromBody] PostDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            return Ok(await _postBL.Create(postDto));
        }
        [HttpGet]
        [Route("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _postBL.GetPostById(id));
            
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] PostDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            return Ok(await _postBL.Update(postDto));
        }
        [HttpGet]
        [Route("Detail/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _postBL.GetPostById(id));
        }

        [HttpGet]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _postBL.Delete(id));
        }

        [HttpGet]
        [Route("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            return Ok(await _postBL.GetAllCategories());
        }

        [HttpGet]
        [Route("GetPopularTags")]
        public async Task<IActionResult> GetPopularTags()
        {
            return Ok(await _postBL.GetPopularTags());
        }
    }
}
