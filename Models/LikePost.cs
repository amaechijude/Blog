using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class LikePost
    {
        [Key]
        public int Id {get; set;}
        public int UserId {get; set;}
        public int PostId {get; set;}
    }
}