using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommentService.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public int ExternalPostId { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(200)]
        public string Content { get; set; }
        public int UserId { get; set; }
        public int LikeCount { get; set; }
    }
}
