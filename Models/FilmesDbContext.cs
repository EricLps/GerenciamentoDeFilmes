using Microsoft.EntityFrameworkCore;

namespace FilmesAPI.Models
{
    public class FilmesDbContext : DbContext
    {
        public FilmesDbContext(DbContextOptions<FilmesDbContext> options) : base(options)
        {
        }

        public DbSet<Filme> Filmes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Filme>()
                .Property(f => f.Titulo)
                .IsRequired();
        }
    }
}