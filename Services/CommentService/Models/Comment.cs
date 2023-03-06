using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CommentService.Models
{
    public class Comment
    {
        public Comment()
        {
            CreatedAt = DateTime.Now;
            Content = string.Empty;
        }

        public int CommentId { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public DateTime CreatedAt { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(200)]
        public string Content { get; set; }

    }
}
