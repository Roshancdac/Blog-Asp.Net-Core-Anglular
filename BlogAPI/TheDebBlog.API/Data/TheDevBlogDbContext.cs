using Microsoft.EntityFrameworkCore;
using TheDebBlog.API.Models.Entities;

namespace TheDebBlog.API.Data
{
    public class TheDevBlogDbContext : DbContext
    {
        public TheDevBlogDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Post> Posts { get; set; }

    }
}
