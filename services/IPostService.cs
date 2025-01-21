using Blog.DTOs;

namespace Blog.services
{
    public interface IPostService
    {
        Task<PostViewDTO?> CreatePostAsync(CreatePostDTO createPost, HttpRequest request);
        Task<IEnumerable<PostViewDTO>> GetAllPostAsync();
        Task<PostViewDTO?> GetPostByIdAsync(int Id);
        Task<PostViewDTO> UpdatePostAsync(int Id, UpdatePostDTO updatePost,HttpRequest request);
        Task DeletePostAsync(int Id);
        
    }
}