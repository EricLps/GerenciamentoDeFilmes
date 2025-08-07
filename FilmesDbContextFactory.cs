using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using FilmesAPI.Models;

namespace FilmesAPI
{
    public class FilmesDbContextFactory : IDesignTimeDbContextFactory<FilmesDbContext>
    {
        public FilmesDbContext CreateDbContext(string[] args)
        {
            var connectionString = "Server=localhost\\SQLEXPRESS;Database=FilmesDB;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;";

            var optionsBuilder = new DbContextOptionsBuilder<FilmesDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new FilmesDbContext(optionsBuilder.Options);
        }
    }
}