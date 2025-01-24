using System.Security.Claims;
using Blog.DTOs;
using Blog.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController(IPostService postService) : ControllerBase
    {
        private readonly IPostService _postService = postService;
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllPostsAsync()
        {
            try
            {
                var posts = await _postService.GetAllPostAsync();
                if (!posts.Any())
                    return NotFound(new { message = "No post found" });
                return Ok(posts);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById([FromRoute] int id)
        {
            try { return Ok(await _postService.GetPostByIdAsync(id)); }
            catch (KeyNotFoundException ex) { return BadRequest(ex.Message); }
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostDTO createPost)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userid = Convert.ToInt32(id);
            return Ok(await _postService.CreatePostAsync(userid, createPost, Request));
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromForm] UpdatePostDTO updatePost)
        {
            try { return Ok(await _postService.UpdatePostAsync(id, updatePost, Request)); }
            catch (KeyNotFoundException ex) { return BadRequest(ex.Message); }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostByIdAsync(int id)
        {
            try
            {
                await _postService.DeletePostAsync(id);
                return Ok(new { message = "Post is deleted" });
            }
            catch (KeyNotFoundException ex) { return BadRequest(ex.Message); }

        }
    }
}