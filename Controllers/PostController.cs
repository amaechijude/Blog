using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.DataContext;
using Blog.DTOs;
using Blog.Models;
using Blog.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController(PostRepository postRepository) : ControllerBase
    {
       private readonly PostRepository _postRepository = postRepository;

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
           var posts = await _postRepository.GetAllPosts();
           if (posts is null)
                return NotFound("Posts not found");
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
           var post = await _postRepository.GetPostById(id);
           if (post is null)
                return NotFound("Post is deleted or does not exist");

            return Ok(post);        
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDTO createPost)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var post = await _postRepository.CreatePostAsync(createPost, Request);
            if (post is null)
                return BadRequest("Post  creation failed");

            return Ok(post);
        }
    }
}