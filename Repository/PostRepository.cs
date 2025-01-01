using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.DataContext;
using Blog.Model;
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
            if (!posts.Any())
                return null;
            return posts;
        }

        public async Task<Post?> GetPostById(int Id)
        {
            var post = await _context.Posts.FindAsync(Id);
            if (post is null)
                return null;

            return post;
        }
    }
}