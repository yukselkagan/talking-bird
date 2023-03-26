using PostService.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostService.Dtos
{
    public class PostDto
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool HaveRepost { get; set; }
        public int RepostId { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public int RepostCount { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }

        public bool UserLiked { get; set; } = false;
    }
}
