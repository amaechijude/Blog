using Blog.DTOs;

namespace Blog.Repository
{
    public interface IPostRepository
    {
        Task<PostViewDTO> CreatePostAsync(CreatePostDTO createPost, HttpRequest request);
        Task<IEnumerable<PostViewDTO>> GetAllPostAsync();
        Task<PostViewDTO> GetPostByIdAsync(int Id);
        Task<PostViewDTO> UpdatePostAsync(int Id, UpdatePostDTO updatePost, HttpRequest request);
        Task<string> DeletePostAsync(int Id);
    }
}
