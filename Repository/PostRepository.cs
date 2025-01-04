using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.DataContext;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repository
{
    public class PostRepository(AppDbContext context)
    {
        private readonly AppDbContext _context = context;

        public async Task<IEnumerable<Post>?> GetAllPosts()
        {
            var posts = await _context.Posts
                .Where(p => !p.IsDeleted)
                .ToListAsync();
            if (posts is null || !posts.Any())
                return null;
            return posts is null || !posts.Any() ? null : posts;
        }

        public async Task<Post?> GetPostById(int Id)
        {
            var post = await _context.Posts.FindAsync(Id);

            return post is null || post.IsDeleted ? null : post;
        }
    }
}