using System.ComponentModel.DataAnnotations;
// using Blog.Models;

namespace Blog.DTOs
{
    public class CreatePostDTO
    {
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Content { get; set; }
        public int UserId {get; set;}
        public IFormFile? Image { get; set; }
    }

    public class UpdatePostDTO
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public IFormFile? Image { get; set; }
    }
    public class PostViewDTO
    {
        public int Id {  get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public int Likes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public int? UserId { get; set; }
        // public User? User {get; set;}
    }

}
