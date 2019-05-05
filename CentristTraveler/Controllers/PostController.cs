using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentristTraveler.BusinessLogic.Interfaces;
using CentristTraveler.Dto;
using CentristTraveler.Model;
using Microsoft.AspNetCore.Mvc;

namespace CentristTraveler.Controllers
{

    [Route("api/[controller]")]
    public class PostController : Controller
    {
        private IPostBL _postBL;
        public PostController(IPostBL postBL)
        {
            _postBL = postBL;
        }

        [HttpGet]
        [Route("GetAllPosts")]
        public IEnumerable<PostDto> GetAllPosts()
        {
            List<Post> posts = _postBL.GetAllPosts();
            List<PostDto> postDtos = new List<PostDto>();
            foreach (Post post in posts)
            {
                PostDto postDto = new PostDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Body = post.Body,
                    ThumbnailPath = post.ThumbnailPath,
                    CreatedDate = post.CreatedDate,
                    UpdatedDate = post.UpdatedDate
                };
                postDtos.Add(postDto);
            }
            return postDtos;
        }

        [HttpPost]
        [Route("AddPost")]
        public IActionResult AddPost([FromBody] PostDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Post post = new Post
            {
                Title = postDto.Title,
                Body = postDto.Body,
                ThumbnailPath = postDto.ThumbnailPath
            };
            
            return Ok(_postBL.Create(post));
        }
        [HttpGet]
        [Route("Update/{id}")]
        public IActionResult Update(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Post post = _postBL.GetPostById(id);
            PostDto postDto = new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                ThumbnailPath = post.ThumbnailPath,
                CreatedDate = post.CreatedDate,
                CreatedBy = post.CreatedBy,
                UpdatedDate = post.UpdatedDate,
                UpdatedBy = post.UpdatedBy
            };
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
            Post post = new Post
            {
                Id = postDto.Id,
                Title = postDto.Title,
                Body = postDto.Body,
                ThumbnailPath = postDto.ThumbnailPath,
                CreatedDate = postDto.CreatedDate,
                CreatedBy = postDto.CreatedBy,
                UpdatedDate = postDto.UpdatedDate,
                UpdatedBy = postDto.UpdatedBy
            };
           
            return Ok(_postBL.Update(post));
        }
        [HttpGet]
        [Route("Detail/{id}")]
        public IActionResult Detail(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Post post = _postBL.GetPostById(id);
            PostDto postDto = new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                ThumbnailPath = post.ThumbnailPath,
                CreatedDate = post.CreatedDate,
                CreatedBy = post.CreatedBy,
                UpdatedDate = post.UpdatedDate,
                UpdatedBy = post.UpdatedBy
            };
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

    }
}
