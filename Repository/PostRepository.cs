using Blog.DataContext;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repository
{
    public class PostRepository(AppDbContext context) : IPostRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<User?> GetUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user is null? null : user;

        }
        public async Task<Post> CreatePostAsync(Post createPost)
        {
            _context.Posts.Add(createPost);
            await _context.SaveChangesAsync();
            return createPost;
        }
        public async Task<IEnumerable<Post>> GetAllPostAsync()
        {
            var allPosts = await _context.Posts.ToListAsync();
            return allPosts;
        }
        public async Task<Post> GetPostByIdAsync(int Id)
        {
            return await _context.Posts.FindAsync(Id)
                ?? throw new KeyNotFoundException("Post not found or is deleted");
        }
        public async Task<Post> UpdatePostAsync(Post updatePost)
        {
            _context.Entry(updatePost).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return updatePost;
        }
        public async Task DeletePostAsync(int Id)
        {
            var existingPost = await GetPostByIdAsync(Id)
                ?? throw new KeyNotFoundException("Post not found or is deleted");
            existingPost.IsDeleted = true;
        }
        public async Task<string?> SavePostImageAsync(IFormFile imageFile, HttpRequest request)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;
                
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Products");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileName = $"{Guid.NewGuid()}_{Path.GetExtension(imageFile.FileName)}".Replace(" ", "");
            var filePath = Path.Combine(uploadPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            var imageUrl = $"{request.Scheme}://{request.Host}/Uploads/Posts/{fileName}";
            return imageUrl;
        }

         }
}