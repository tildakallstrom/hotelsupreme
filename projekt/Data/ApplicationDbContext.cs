using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using projekt.Models;

namespace projekt.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<projekt.Models.Posts> Posts { get; set; }
        public DbSet<projekt.Models.Rooms> Rooms { get; set; }
        public DbSet<projekt.Models.About> About { get; set; }
    }
}