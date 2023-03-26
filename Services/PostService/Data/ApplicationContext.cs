using Microsoft.EntityFrameworkCore;
using PostService.Model;

namespace PostService.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder
            //    .Entity<Post>()
            //    .HasOne(e => e.User)
            //    .WithMany()
            //    .HasForeignKey("UserId")
            //    .HasPrincipalKey("ExternalUserId")
            //    .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<Post>()
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey("UserId")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<Post>().HasOne(e => e.User).WithMany().

            //.OnDelete(DeleteBehavior.NoAction)




        }



    }
}
