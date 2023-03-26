using Microsoft.EntityFrameworkCore;
using TrendService.Models;

namespace TrendService.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<Trend> Trends { get; set; }

    }
}
