using Blog.DTOs;
using Blog.Models;

namespace Blog.Repository
{
    public interface IPostRepository
    {
        Task<User?> GetUser(int userId);
        Task<Post> CreatePostAsync(Post createPost);
        Task<IEnumerable<Post>> GetAllPostAsync();
        Task<Post> GetPostByIdAsync(int Id);
        Task<Post> UpdatePostAsync(Post updatePost);
        Task DeletePostAsync(int Id);
        Task<string?> SavePostImageAsync(IFormFile imageFile, HttpRequest request);
    }
}
