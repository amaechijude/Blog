using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.DataContext;
using Blog.DTOs;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repository
{
    public interface IPostRepository
    {
        Task<PostViewDTO> CreatePostAsync(CreatePostDTO createPost);
        Task<IEnumerable<PostViewDTO>> GetAllPostAsync();
        Task<PostViewDTO> GetPostByIdAsync(int Id);
        Task<PostViewDTO> UpdatePostAsync(int Id, UpdatePostDTO updatePost);
        Task<string> DeletePostAsync(int Id);
    }
    public class PostRepository(AppDbContext context)// : IPostRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<PostViewDTO?> CreatePostAsync(CreatePostDTO createPost, HttpRequest request)
        {
            if (string.IsNullOrWhiteSpace(createPost.Content) || string.IsNullOrWhiteSpace(createPost.Title))
                return null;
            
            var user = await _context.Users.FindAsync(createPost.UserId);
            var post = new Post()
            {
                Title = createPost.Title,
                Content = createPost.Content,
                UserId = user is null ? null : createPost.UserId,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.MinValue,
            };
            if (createPost.Image != null)
            {
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Posts");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var fileName = $"{Guid.NewGuid()}_{Path.GetExtension(createPost.Image.FileName)}".Replace(" ", "");
                string filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await createPost.Image.CopyToAsync(stream);
                }
                var imgUrl = $"{request.Scheme}://{request.Host}/Uploads/Posts/{fileName}";
                post.ImageUrl = imgUrl;
            }
            user?.Posts.Add(post); // add post to user if user exists
                
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            var postView = new PostViewDTO()
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                UserId = post.UserId,
                CreatedAt = post.CreatedAt
            };
            return postView;
        }
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