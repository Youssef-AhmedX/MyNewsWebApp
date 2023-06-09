using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyNewsApi.Models;

namespace MyNewsApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<News> News { get; set; }
        public DbSet<Author> Authors { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Author>()
                .HasMany(a => a.news)
                .WithOne(n => n.Author)
                .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(builder);

        }
    }
}
