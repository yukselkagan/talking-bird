using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostService.Model
{
    public class Post
    {
        public Post()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
        [Key]
        public int PostId { get; set; }  
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UserId { get; set; }
        public bool HaveRepost { get; set; }
        public int RepostId { get; set; }
        public int LikeCount { get; set; }
        //public int CommentCount { get; set; }
        //public int RepostCount { get; set; }
        //[ForeignKey("UserId")]
        //public User? User { get; set; }
    }
}
