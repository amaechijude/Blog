using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Model
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
        public string? AvatarURL { get; set; }
        public bool IsAdmin { get; set; } = false;
        public DateTime JoinedOn { get; set; }
        public ICollection<Post> Posts { get; set; } = [];
    }
}