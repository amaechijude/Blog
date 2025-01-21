using System.ComponentModel.DataAnnotations;

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
        public string? FullName { get; set; }
    }

    public class UpdateUserDTO
    {
        public string? FullName { get; set; }
    }

    public class JwtUserViewDto
    {
        public int UserId {get; set;}
        public string? Email {get; set; }
        public string? FullName {get; set;}
        public string? JwtToken {get; set;}
    }
}