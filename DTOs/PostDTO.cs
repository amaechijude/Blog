namespace Blog.DTOs
{
    public class CreatePostDTO
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public IFormFile? Image { get; set; }
    }

    public class UpdatePostDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public IFormFile? Image { get; set; }
    }

}
