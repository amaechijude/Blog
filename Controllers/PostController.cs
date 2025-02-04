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
            var posts = await _postService.GetAllPostAsync();
            if (!posts.Any())
                return NotFound(new { message = "No post found" });
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById([FromRoute] int id)
        {
            return Ok(await _postService.GetPostByIdAsync(id));
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
            return Ok(await _postService.UpdatePostAsync(id, updatePost, Request));
        }

        [Authorize]
        [HttpDelete("delete/{postid}")]
        public async Task<IActionResult> DeletePostByIdAsync([FromRoute] int postid)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userid is null)
                return BadRequest("User is not authenticated or not found");
            int userId = Convert.ToInt32(userid);
            await _postService.DeletePostAsync(userId, postid);
            return Ok(new { message = "Post is deleted" });
        }

        [Authorize]
        [HttpPost("like/{postId}")]
        public async Task<IActionResult> LikePostAsync([FromRoute] int postId)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userid is null)
                return BadRequest("User is not authenticated or not found");

            int userId = Convert.ToInt32(userid);
            return Ok(await _postService.LikePostAsync(userId, postId));
        }
    }
}