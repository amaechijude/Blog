using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.DataContext;
using Blog.DTOs;
using Blog.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController(AppDbContext context) : ControllerBase
    {
       private readonly AppDbContext _context = context;
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            var posts = await _context.Posts
                .Where(post => !post.IsDeleted)
                .ToListAsync();

            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post is null || post.IsDeleted)
                return NotFound("Post does not exists or is deletet");

            return Ok(post);
        }
        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
        }
        [HttpPut("update{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromForm] UpdatePostDTO post)
        {
            if (id != post.Id)
            {
                return BadRequest();
            }
            _context.Entry(post).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null || post.IsDeleted)
                return BadRequest("Post does not exist or Is deleted");

            post.IsDeleted = true;
            await _context.SaveChangesAsync();
            return Ok("Post Deleted");
        }
    }
}