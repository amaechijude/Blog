namespace Blog.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty; // Store user ID
        public DateTime Expires { get; set; }
        public bool IsRevoked { get; set; }
    }

}