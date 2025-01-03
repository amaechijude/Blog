using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.DTOs
{
    public class UserDTO
    {
        [EmailAddress]
        [Required]
        public string? Email { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 3)]
        public string? Password { get; set; }
    }

    public class UpdateUserDTO
    {
        public string? FullName { get; set; }
        public IFormFile? Avatar { get; set; }
    }
}