using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Blog.Models;

namespace Blog.DTOs
{
    public class UserProfileDTO
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public DateTime JoinedOn { get; set; }
        public List<PostViewDTO> Posts {get; set;} = [];

    }
    public class LoginUserDTO
    {
        [EmailAddress]
        [Required]
        public string? Email { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 3)]
        public string? Password { get; set; }
    }
    public class RegisterUserDTO : LoginUserDTO
    {
        [Required]
        public string? Username { get; set; }
    }

    public class UpdateUserDTO
    {
        public string? FullName { get; set; }
    }
}