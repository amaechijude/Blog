using Blog.DTOs;
using Blog.Models;
using Blog.Repository;

namespace Blog.services
{
    public class PostService(IPostRepository postRepository) : IPostService
    {
        private readonly IPostRepository _postRepository = postRepository;
        public async Task<PostViewDTO?> CreatePostAsync(CreatePostDTO createPost, HttpRequest request)
        {
            if (string.IsNullOrWhiteSpace(createPost.Content) || string.IsNullOrWhiteSpace(createPost.Title))
                return null;
            var user = await _postRepository.GetUser(createPost.UserId);
            if (user is null)
                return null;
            var imageUrl = createPost.Image is null ? null : await _postRepository.SavePostImageAsync(createPost.Image, request);
            var post = new Post
            {
                Title = createPost.Title,
                Content = createPost.Content,
                CreatedAt = DateTime.UtcNow,
                UserId = user.Id,
                ImageUrl = imageUrl,
                LastUpdatedAt = DateTime.MinValue,
                Likes = 0,
                IsDeleted = false
            };
            var create = await _postRepository.CreatePostAsync(post);
            return new PostViewDTO
            {
                Id = create.Id,
                Title = create.Title,
                Content = create.Content,
                ImageUrl = create.ImageUrl,
                Likes = create.Likes,
                CreatedAt = create.CreatedAt,
                LastUpdatedAt = create.LastUpdatedAt,
                UserId = create.UserId
            };
        }

        public async Task<IEnumerable<PostViewDTO>> GetAllPostAsync()
        {
            var posts = await _postRepository.GetAllPostAsync();
            return posts.Select(p => new PostViewDTO
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                ImageUrl = p.ImageUrl,
                Likes = p.Likes,
                CreatedAt = p.CreatedAt,
                LastUpdatedAt = p.LastUpdatedAt,
                UserId = p.UserId
            });
        }

        public async Task<PostViewDTO?> GetPostByIdAsync(int Id)
        {
            var post = await _postRepository.GetPostByIdAsync(Id)
                ?? throw new KeyNotFoundException("Post not found or is deleted");
            return new PostViewDTO
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                ImageUrl = post.ImageUrl,
                Likes = post.Likes,
                CreatedAt = post.CreatedAt,
                LastUpdatedAt = post.LastUpdatedAt,
                UserId = post.UserId
            };
        }

        public async Task<PostViewDTO> UpdatePostAsync(int Id, UpdatePostDTO updatePost, HttpRequest request)
        {
            var existingPost = await _postRepository.GetPostByIdAsync(Id);
            existingPost.Title = updatePost.Title;
            existingPost.Content = updatePost.Content;
            
            var imageUrl = updatePost.Image is null ? null : await _postRepository.SavePostImageAsync(updatePost.Image, request);
            if (imageUrl != null)
                existingPost.ImageUrl = imageUrl;

            var update = await _postRepository.UpdatePostAsync(existingPost);
            return new PostViewDTO
            {
                Id = update.Id,
                Title = update.Title,
                Content = update.Content,
                ImageUrl = update.ImageUrl,
                Likes = update.Likes,
                CreatedAt = update.CreatedAt,
                LastUpdatedAt = update.LastUpdatedAt,
                UserId = update.UserId
            };
        }

        public async Task DeletePostAsync(int Id)
        {
            try
            {
                await _postRepository.DeletePostAsync(Id);
            }
            catch (KeyNotFoundException) { throw new KeyNotFoundException("Post not found or is already deleted"); }
        }
    }
}