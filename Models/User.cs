using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class User
    {
        public int Id { get; set; }
        [EmailAddress]
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? PasswordHash { get; set; }
        public string? FullName { get; set; }
        public DateTime JoinedOn { get; set; }
        public List<Post> Posts { get; set; } = [];
    }
}