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
            return user is null ? null : user;

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
        public async Task DeletePostAsync(int userId, int postId)
        {
            var existingPost = await GetPostByIdAsync(postId)
                ?? throw new KeyNotFoundException("Post not found or is deleted");
            if (existingPost.UserId != userId)
                throw new KeyNotFoundException("You are not the author");
            if (existingPost.IsDeleted)
                throw new KeyNotFoundException("Post is already deleted");

            existingPost.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
        public async Task<string?> SavePostImageAsync(IFormFile imageFile, HttpRequest request)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Posts");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileName = $"{Guid.NewGuid()}_{Path.GetExtension(imageFile.FileName)}".Replace(" ", "");
            var filePath = Path.Combine(uploadPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            var imageUrl = $"{request.Scheme}://{request.Host}/Uploads/Posts/{fileName}";
            return imageUrl;
        }

        public async Task<int> LikePostAsync(int userId,  int postId)
        {
            var post = await _context.Posts.FindAsync(postId);
            var user = await _context.Users.FindAsync(userId);
            if (post is null || user is null)
                throw new Exception("Post no longer exist");

            var like = await _context.LikePosts.Where(l => l.UserId == userId && l.PostId == postId).ToListAsync();
            if (like.Count > 0)
                throw new Exception("You have already liked the post");

            var newLike = new LikePost {UserId=userId, PostId=postId};
            _context.LikePosts.Add(newLike);
            post.Likes += 1;
            await _context.SaveChangesAsync();
            return post.Likes;
        }
    }
}
