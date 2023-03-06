using CommentService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommentService.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        DbSet<Comment> Comments { get; set; }
        DbSet<Post> Posts { get; set; }


    }
}
