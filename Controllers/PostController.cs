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
        
        [HttpGet("all")]
        public async Task<IActionResult> GetAllPostsAsync()
        {
            try
            {
                var posts = await _postService.GetAllPostAsync();
                if (!posts.Any())
                    return NotFound(new { message = "No post found" });
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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

        [Authorize]
        [HttpDelete("delete/{postid}")]
        public async Task<IActionResult> DeletePostByIdAsync([FromRoute] int postid)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userid is null)
                return BadRequest("User is not authenticated or not found");
            int userId = Convert.ToInt32(userid);
            try
            {
                await _postService.DeletePostAsync(userId, postid);
                return Ok(new { message = "Post is deleted" });
            }
            catch (KeyNotFoundException ex) { return BadRequest(ex.Message); }
        }

        [Authorize]
        [HttpPost("like/{postId}")]
        public async Task<IActionResult> LikePostAsync([FromRoute] int postId)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userid is null)
                return BadRequest("User is not authenticated or not found");
            int userId = Convert.ToInt32(userid);
            try
            {
                return Ok(await _postService.LikePostAsync(userId, postId));
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    status = "faliled",
                    message = ex.Message
                });
            }
        }
    }
}