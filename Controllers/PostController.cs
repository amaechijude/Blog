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

        [HttpGet]
        public async Task<IActionResult> GetAllPostsAsync()
        {
           var posts = await _postService.GetAllPostAsync();
           if (!posts.Any())
                return NotFound( new {message = "No post found"});
            return Ok(posts);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetPostById([FromRoute]int id)
        {
            try { return Ok(await _postService.GetPostByIdAsync(id)); }
            catch(KeyNotFoundException ex) { return BadRequest(ex.Message); }    
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDTO createPost)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _postService.CreatePostAsync(createPost, Request));
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
                return Ok(new {message = "Post is deleted"});
             }
            catch (KeyNotFoundException ex) { return BadRequest(ex.Message); }
             
        }
    }
}