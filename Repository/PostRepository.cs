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
    public class PostRepository(AppDbContext context) : IPostRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<PostViewDTO> CreatePostAsync(CreatePostDTO createPost, HttpRequest request)
        {
            if (string.IsNullOrWhiteSpace(createPost.Content) || string.IsNullOrWhiteSpace(createPost.Title))
                return new PostViewDTO {};

            var user = _context.Users.FirstOrDefault(u => u.Id == createPost.UserId);
            if (user is null)
                return new PostViewDTO {};
            var post = new Post()
            {
                Title = createPost.Title,
                Content = createPost.Content,
                CreatedAt = DateTime.UtcNow,
                UserId = createPost.UserId,
                LastUpdatedAt = DateTime.MinValue,
                Likes = 0,
                IsDeleted = false
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

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            var postView = new PostViewDTO()
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                UserId = post.UserId,
                CreatedAt = post.CreatedAt,
            };
            return postView;
        }
        public async Task<IEnumerable<PostViewDTO>> GetAllPostAsync()
        {
            var posts = await _context.Posts
                .Where(p => !p.IsDeleted)
                .Select(p => new PostViewDTO
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    ImageUrl = p.ImageUrl,
                    Likes = p.Likes,
                    CreatedAt = p.CreatedAt,
                    LastUpdatedAt = p.LastUpdatedAt,
                    UserId = p.UserId

                })
                .ToListAsync();

            if (posts is null || posts.Count == 0)
                return [];
            return posts;
        }

        public async Task<PostViewDTO> GetPostByIdAsync(int Id)
        {
            var post = await _context.Posts.FindAsync(Id);
            if (post is null)
                return new PostViewDTO {};
            var postView = new PostViewDTO()
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                UserId = post.UserId,
                CreatedAt = post.CreatedAt,
            };
            return postView;
        }

        public async Task<PostViewDTO> UpdatePostAsync(int id, [FromBody] UpdatePostDTO updatePost, HttpRequest request)
        {
            var existingPost = await _context.Posts.FindAsync(id);
            if (existingPost is null)
                return new PostViewDTO {};

            if (!string.IsNullOrWhiteSpace(updatePost.Content))
                existingPost.Content = updatePost.Content;
            existingPost.LastUpdatedAt = DateTime.UtcNow;

            if (updatePost.Image != null)
            {
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Posts");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var fileName = $"{Guid.NewGuid()}_{Path.GetExtension(updatePost.Image.FileName)}".Replace(" ", "");
                string filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await updatePost.Image.CopyToAsync(stream);
                }
                var imgUrl = $"{request.Scheme}://{request.Host}/Uploads/Posts/{fileName}";
                existingPost.ImageUrl = imgUrl;
                existingPost.LastUpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            var postView = new PostViewDTO()
            {
                Id = existingPost.Id,
                Title = existingPost.Title,
                Content = existingPost.Content,
                CreatedAt = existingPost.CreatedAt,
                LastUpdatedAt = existingPost.LastUpdatedAt,
                Likes = existingPost.Likes,
                UserId = existingPost.UserId,
            };
            return postView;
        }

        public async Task<string> DeletePostAsync(int id)
        {
            var postToDelete = await _context.Posts.FindAsync(id);
            if (postToDelete is null || !postToDelete.IsDeleted)
                return "Post does not exist or is deleted";

            postToDelete.IsDeleted = true;
            await _context.SaveChangesAsync();
            return "Post deleted";
        }
        public async Task<string> LikePostAsync(int id)
        {
            var postToLike = await _context.Posts.FindAsync(id);
            if (postToLike is null || !postToLike.IsDeleted)
                return "Post does not exist or is deleted";

            postToLike.Likes += 1;
            await _context.SaveChangesAsync();
            return "Post Liked";
        }
        public async Task<string> UnLikePostAsync(int id)
        {
            var postToLike = await _context.Posts.FindAsync(id);
            if (postToLike is null || !postToLike.IsDeleted)
                return "Post does not exist or is deleted";

            if (postToLike.Likes >= 0)
                postToLike.Likes -= 1;
            await _context.SaveChangesAsync();
            return "Post unliked";
        }
    }
}